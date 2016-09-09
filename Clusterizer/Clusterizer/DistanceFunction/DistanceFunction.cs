using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clusterizer.Entities;

namespace Clusterizer.DistanceFunction
{
    /// <summary>
    /// Реализация IDistanceFunction. Использует вероятностную модель
    /// </summary>
    public class DistanceFunction : IDistanceFunction
    {
        private Dictionary<Tuple<string, string>, double> wordCounts = new Dictionary<Tuple<string, string>, double>();
        private double allWordsCount;

        public DistanceFunction(IEnumerable<List<string>> attributes)
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
            double result = 0;
            if (entity1.TextAttributes.Count == entity2.TextAttributes.Count)
            {
                result = 0;
                for (int i = 0; i < entity1.TextAttributes.Count; i++)
                {
                    var p12 = CombinedSequenceProbability(entity1.TextAttributes[i], entity2.TextAttributes[i]);
                    if (p12 != 0)
                    {
                        double p1 = SequenceProbability(entity1.TextAttributes[i]);
                        double p2 = SequenceProbability(entity2.TextAttributes[i]);
                        double npmi = Math.Log(p12 / (p1 * p2)) / Math.Log(p12);
                        result += ((-npmi + 1)/2);
                    }
                }
                result /= entity1.TextAttributes.Count;
                if (double.IsInfinity(result))
                    result = -1;
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

        private double SequenceProbability(List<string> sequence, string prevWord = null)
        {
            double result = 1.0;
            for (int i = 0; i < sequence.Count; i++)
            {
                result *= TwoWordsProbabilty(prevWord, sequence[i]);
                prevWord = sequence[i];
            }
            return result;
        }

        private double CombinedSequenceProbability(List<string> seq1, List<string> seq2)
        {
            double result = 1.0;
            var length = Math.Min(seq1.Count, seq2.Count);
            string prevWord = null;
            for (int i = 0; i < length; i++)
            {
                if (seq1[i] == seq2[i])
                {
                    result *= TwoWordsProbabilty(prevWord, seq1[i]);
                }
                else
                {
                    result *= SingleWordProbability(seq1[i]) * SingleWordProbability(seq2[i]);
                }
                prevWord = seq1[i];
            }
            result *= SequenceProbability(seq1.Skip(length).ToList(), seq1[length-1]);
            result *= SequenceProbability(seq2.Skip(length).ToList(), seq2[length-1]);
            return result;
        }

        private double TwoWordsProbabilty(string prevWord, string word)
        {
            double result = 0;
            if (wordCounts.Keys.Contains(new Tuple<string, string>(prevWord, word)))
            {
                result = (wordCounts[new Tuple<string, string>(prevWord, word)] /
                    ((prevWord == null) ? allWordsCount : wordCounts[new Tuple<string, string>(null, word)]));
            }
            else
            {
                result = SingleWordProbability(word);
            }
            return result;
        }

        private double SingleWordProbability(string word)
        {
            double result = 0;
            if (wordCounts.Keys.Contains(new Tuple<string, string>(null, word)))
                result = wordCounts[new Tuple<string, string>(null, word)] / allWordsCount;
            return result;
        }
    }
}
