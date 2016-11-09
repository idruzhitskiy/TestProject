using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClusterizerTests.AutomationTests
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    class ClusterizerTestAttribute : Attribute
    {
        public ClusterizerTestAttribute(params string[] entities)
        {
            Entities = entities.Select(s =>
            {
                var num = int.Parse(s.Split(' ')[0]);
                var words = s
                    .Substring(s.IndexOf(s.Split(' ')[1]))
                    .Split('\n').Select(s1 => s1.Split(' ').Where(s2 => !string.IsNullOrWhiteSpace(s2)).ToList())
                    .ToList();
                return new Tuple<int, List<List<string>>>(num, words);
            }).ToList();
            NumOfClusters = Entities.Select(t => t.Item1).Distinct().Count();
        }

        public List<Tuple<int, List<List<string>>>> Entities { get; private set; }
        public int NumOfClusters { get; private set; }
    }
}
