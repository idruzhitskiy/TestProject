using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clusterizer.Entities;

namespace Clusterizer.DistanceFunction
{
    class NpmiDistanceFunction : IDistanceFunction
    {
        Dictionary<Tuple<string, string>, int> probabilites = new Dictionary<Tuple<string, string>, int>();

        public NpmiDistanceFunction(IEnumerable<List<string>> attributes)
        {
            foreach (var attribute in attributes)
            {
                IncProbability(null, attribute[0]);
                for (int i = 1; i < attribute.Count; i++)
                {
                    IncProbability(attribute[i-1], attribute[i]);
                    IncProbability(null, attribute[i]);
                }
            }
        }

        public double Distance(Entity entity1, Entity entity2)
        {
            throw new NotImplementedException();
        }

        private void IncProbability(string left, string right)
        {
            var key = new Tuple<string, string>(left, right);
            if (!probabilites.Keys.Contains(key))
                probabilites.Add(key, 0);
            probabilites[key]++;
        }
    }
}
