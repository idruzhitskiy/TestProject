using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clusterizer.Entities;
using Clusterizer.EntitiesReader;

namespace Clusterizer.DistanceFunction
{
    public class DistanceFunction : IDistanceFunction
    {
        private readonly IEntitiesReader reader;
        private List<string> words;

        public DistanceFunction(IEntitiesReader reader)
        {
            this.reader = reader;
            words = new List<string>();
            foreach (var attrsList in reader.Entities.Select(e => e.TextAttributes))
            {
                foreach (var attr in attrsList)
                {
                    foreach (var word in attr)
                    {
                        if (!words.Contains(word))
                            words.Add(word);
                    }
                }
            }
        }

        public double Distance(IEntity entity1, IEntity entity2)
        {
            if (entity1.TextAttributes.Count != entity2.TextAttributes.Count)
                throw new ArgumentException("У объектов различное число атрибутов");

            double result = 0;
            for (int i = 0; i < entity1.TextAttributes.Count; i++)
            {
                List<int> vector1 = FillWordCountVector(entity1.TextAttributes[i]);
                List<int> vector2 = FillWordCountVector(entity2.TextAttributes[i]);
                result += EuclidDistance(vector1, vector2);
            }
            result /= entity1.TextAttributes.Count;
            return result;
        }

        private double EuclidDistance(List<int> vector1, List<int> vector2)
        {
            if (vector1.Count != vector2.Count)
                throw new ArgumentException("У векторов различные длины");

            double sum = 0;
            for (int i=0; i<vector1.Count; i++)
            {
                sum += Math.Abs(vector1[i] - vector2[i]) * Math.Abs(vector1[i] - vector2[i]);
            }
            return Math.Sqrt(sum);
        }

        private List<int> FillWordCountVector(List<string> list)
        {
            List<int> result = new List<int>();
            foreach (var word in words)
            {
                if (list.Contains(word))
                {
                    result.Add(list.Where(s => s == word).Count());
                }
                else
                {
                    result.Add(0);
                }
            }
            return result;
        }
    }
}
