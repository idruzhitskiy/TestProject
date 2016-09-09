﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clusterizer.Entities;

namespace Clusterizer.DistanceFunction
{
    public class NpmiDistanceFunction : IDistanceFunction
    {
        private Dictionary<Tuple<string, string>, double> wordCounts = new Dictionary<Tuple<string, string>, double>();
        private double allWordsCount;

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
            double result = 1.0;
            string prevWord = null;
            for (int i = 0; i < sequence.Count; i++)
            {
                if (wordCounts.Keys.Contains(new Tuple<string, string>(prevWord, sequence[i])))
                {
                    result *= (wordCounts[new Tuple<string, string>(prevWord, sequence[i])] /
                        ((prevWord == null) ? allWordsCount : wordCounts[new Tuple<string, string>(null, sequence[i])]));
                }
                else
                {
                    result *= (wordCounts[new Tuple<string, string>(null, sequence[i])] / allWordsCount);
                }
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
                    result *= (wordCounts[new Tuple<string, string>(prevWord, seq1[i])] /
                        ((prevWord == null) ? allWordsCount : wordCounts[new Tuple<string, string>(null, seq1[i])]));
                }
                else
                {
                    result *= (wordCounts[new Tuple<string, string>(null, seq1[i])] / allWordsCount)
                        * (wordCounts[new Tuple<string, string>(null, seq2[i])] / allWordsCount);
                }
                prevWord = seq1[i];
            }
            return result;
        }
    }
}
