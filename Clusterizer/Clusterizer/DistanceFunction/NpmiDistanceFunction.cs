using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clusterizer.Entities;

namespace Clusterizer.DistanceFunction
{
    public class NpmiDistanceFunction : IDistanceFunction
    {
        private Dictionary<Tuple<string, string>, int> wordCounts = new Dictionary<Tuple<string, string>, int>();
        private int allWordsCount;

        public NpmiDistanceFunction(IEnumerable<List<string>> attributes)
        {
            foreach (var attribute in attributes)
            {
                IncProbability(null, attribute[0]);
                allWordsCount++;
                for (int i = 1; i < attribute.Count; i++)
                {
                    IncProbability(attribute[i - 1], attribute[i]);
                    IncProbability(null, attribute[i]);
                    allWordsCount++;
                }
            }
        }

        public double Distance(Entity entity1, Entity entity2)
        {
            var result = -1.0;
            if (entity1.TextAttributes.Count == entity2.TextAttributes.Count)
            {
                result = 1;
                for (int i=0; i<entity1.TextAttributes.Count; i++)
                {
                }
            }
            return result;
        }

        private void IncProbability(string left, string right)
        {
            var key = new Tuple<string, string>(left, right);
            if (!wordCounts.Keys.Contains(key))
                wordCounts.Add(key, 0);
            wordCounts[key]++;
        }

        private double SequenceProbability(List<string> sequence)
        {
            var result = 1.0;
            string prevWord = null;
            for (int i = 0; i < sequence.Count; i++)
            {
                result *= (wordCounts[new Tuple<string, string>(prevWord, sequence[i])] /
                    ((prevWord == null) ? allWordsCount : wordCounts[new Tuple<string, string>(null, sequence[i])]));
            }
            return result;
        }
    }
}
