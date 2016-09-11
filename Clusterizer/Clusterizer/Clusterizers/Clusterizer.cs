using Clusterizer.Entities;
using Clusterizer.DistanceFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clusterizer.Clusterizers
{
    class Clusterizer
    {
        private double GetCentroid(List<IEntity> entities)
        {
            if (entities.Count == 0)
                throw new Exception("Entities is empty!");

            List<IEntity> mean = new List<IEntity>();

            foreach (IEntity value in entities)
            {

            }


            return 0;
        }

        private bool UpdateMeans(List<IEntity> entities, List<List<IEntity>> clustering, List<IEntity> means)
        {
            return true;
        }

        private bool UpdateClustering(List<IEntity> entities, List<List<IEntity>> clustering, List<IEntity> means)
        {
            return true;
        }

        public List<List<IEntity>> Cluster(List<IEntity> entities, int numberOfClusters)
        {
            if (numberOfClusters <= 0)
                throw new Exception("Number of clusters is negative or equal zero!");

            bool changed = true;
            bool success = true;

            List<List<IEntity>> clustering = new List<List<IEntity>>(numberOfClusters);
            List<IEntity> means = new List<IEntity>(numberOfClusters);

            int count = 10 * entities.Count;
            int currEntity = 0;

            while (changed && success && currEntity < count)
            {
                success = UpdateMeans(entities, clustering, means);
                changed = UpdateClustering(entities, clustering, means);
                currEntity++;
            }

            return clustering;
        }
    }
}
