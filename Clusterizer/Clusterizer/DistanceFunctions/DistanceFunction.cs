using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clusterizer.Entities;
using Clusterizer.EntitiesReaders;

namespace Clusterizer.DistanceFunctions
{
    /// <summary>
    /// Реализация IDistanceFunction с использованием Евклидова расстояния
    /// </summary>
    public class DistanceFunction : IDistanceFunction
    {
        private List<string> words;

        public DistanceFunction(IEntitiesReader reader)
        {
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

        /// <summary>
        /// Функция расстояния
        /// </summary>
        /// <param name="entity1">Сущность</param>
        /// <param name="entity2">Сущность</param>
        /// <returns>{0;1}</returns>
        public double Distance(IEntity entity1, IEntity entity2)
        {
            if (entity1.TextAttributes.Count != entity2.TextAttributes.Count)
                throw new ArgumentException("У объектов различное число атрибутов");

            double result = 0;
            for (int i = 0; i < entity1.TextAttributes.Count; i++)
            {
                var vector1 = FillWordCountVector(entity1.TextAttributes[i]);
                var vector2 = FillWordCountVector(entity2.TextAttributes[i]);
                result += DistanceBetweenVectors(vector1, vector2);
            }
            result /= entity1.TextAttributes.Count;
            return result;
        }

        /// <summary>
        /// Функция нахождения центра кластера
        /// </summary>
        /// <param name="entities">Сущности</param>
        /// <returns>Сущность, являющаяся центром кластера</returns>
        public IEntity Centroid(List<IEntity> entities)
        {
            throw new NotImplementedException();
        }

        private double DistanceBetweenVectors(List<double> vector1, List<double> vector2)
        {
            List<double> resultVector = new List<double>();
            var length = vector1.Count;
            for (int i = 0; i < length; i++)
            {
                resultVector.Add(Math.Abs(vector1[i] - vector2[i]));
            }
            return EuclidDistance(resultVector) / (2 * Math.Sqrt(length * length * 2));
        }

        private double EuclidDistance(List<double> vector)
        {
            double sum = 0;
            foreach (var num in vector)
            {
                sum += (num * num);
            }
            return Math.Sqrt(sum);
        }

        private List<double> NormalizeVector(List<double> vector)
        {
            var length = EuclidDistance(vector);
            double invLength = (length > 0) ? (1.0 / length) : 0;
            return vector.Select(num => num * invLength).ToList();
        }

        private List<double> FillWordCountVector(List<string> list)
        {
            List<double> result = new List<double>();
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
