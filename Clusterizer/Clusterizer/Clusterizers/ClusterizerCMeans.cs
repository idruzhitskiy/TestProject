using Clusterizer.Entities;
using Clusterizer.DistanceFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clusterizer.Clusterizers
{
    class ClusterizerCMeans
    {
        private readonly IDistanceFunction distanceFunction;

        public ClusterizerCMeans(IDistanceFunction distanceFunction)
        {
            this.distanceFunction = distanceFunction;
        }

        public List<List<IEntity>> Clusterize(List<IEntity> entities, int numOfClusters)
        {
            if (numOfClusters <= 0)
                throw new ArgumentException("Отрицательное число кластеров или число кластеров равно нулю!");

            if (entities.Count < numOfClusters)
                throw new ArgumentException("Количество кластеров больше, чем число элементов!");

            if (entities.Count == numOfClusters)
            {
                // return entities;
            }

            List<List<IEntity>> clusters = new List<List<IEntity>>();







            return clusters;
        }
    }
}