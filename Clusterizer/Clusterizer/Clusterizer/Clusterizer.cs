using Clusterizer.Entities;
using Clusterizer.DistanceFunction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clusterizer.Clusterizer
{
    class Clusterizer
    {
        private double GetCentroid(List<Entity> entities)
        {
            if (entities.Count == 0)
                throw new Exception("Entities is empty!");

           List<Entity> mean = new List<Entity>();

            foreach(Entity value in entities)
            {

            }


            return 0;
        }

        private bool UpdateMeans(List<Entity> entities, List<List<Entity>> clustering, List<Entity> means)
        {
            return true;
        }

        private bool UpdateClustering(List<Entity> entities, List<List<Entity>> clustering, List<Entity> means)
        {
            return true;
        }

        public List<List<Entity>> Cluster(List<Entity> entities, int numberOfClusters)
        {
            if (numberOfClusters <= 0)
                throw new Exception("Number of clusters is negative or equal zero!");

            bool changed = true;
            bool success = true;

            List<List<Entity>> clustering = new List<List<Entity>>(numberOfClusters);
            List<Entity> means = new List<Entity>(numberOfClusters);

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
