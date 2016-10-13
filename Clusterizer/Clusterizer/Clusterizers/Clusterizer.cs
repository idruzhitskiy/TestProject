using Clusterizer.Entities;
using Clusterizer.DistanceFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clusterizer.Clusterizers
{
    public class Clusterizer : IClusterizer
    {
        private readonly IDistanceFunction distanceFunction;

        public Clusterizer(IDistanceFunction distanceFunction)
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
                return GetClusters(entities, numOfClusters);

            bool changed = true;
            bool success = true;

            List<int> clustering = InitializationClustering(entities.Count, numOfClusters, 0);
            List<IEntity> means  = InitializationMeans(numOfClusters, entities);

            int count       = 10 * entities.Count;
            int indexEntity = 0;

            while (changed && success && indexEntity < count)
            {
                success = UpdateMeans(entities, clustering, ref means);
                changed = UpdateClustering(entities, ref clustering, means);
                indexEntity++;
            }

            return GetClusters(entities, clustering, numOfClusters);
        }

        private bool UpdateMeans(List<IEntity> entities, List<int> clustering, ref List<IEntity> means)
        {
            int numOfClusters = means.Count;
            int numOfEntities = entities.Count;

            List<int> clusters  = new List<int>(numOfClusters);

            for (int i = 0; i < numOfEntities; i++)
                clusters.Add(0);

            for (int i = 0; i < numOfEntities; i++)
            {
                int index = clustering[i];
                ++clusters[index];
            }

            for (int i = 0; i < numOfClusters; i++)
                if (clusters[i] == 0)
                    return false;
         
            List<IEntity> newMeans = new List<IEntity>(numOfClusters);

            for (int i = 0; i < numOfClusters; i++)
            {
                List<IEntity> tmpMeans = new List<IEntity>(numOfClusters);

                for (int j = 0; j < numOfEntities; j++)
                    if (clustering[j] == i)
                        tmpMeans.Add(entities[j]);

                newMeans.Add(distanceFunction.Centroid(tmpMeans));
            }

            means = newMeans;

            return true;
        }

        private bool UpdateClustering(List<IEntity> entities, ref List<int> clustering, List<IEntity> means)
        {
            int numOfClusters = means.Count;
            int numOfEntities = entities.Count;
            bool changed      = false;

            List<int> newClustering = new List<int>(clustering);

            for (int i = 0; i < numOfEntities; i++)
            {
                List<double> distance = new List<double>(numOfClusters);

                for (int j = 0; j < numOfClusters; j++)
                    distance.Add(distanceFunction.Distance(entities[i], means[j]));

                int index = GetMinIndex(distance);
                if (index != newClustering[i])
                {
                    changed = true;
                    newClustering[i] = index;
                }
            }

            if (changed == false)
                return false;

            List<int> clusters = new List<int>(numOfClusters);

            for (int i = 0; i < numOfClusters; i++)
                clusters.Add(0);

            for (int i = 0; i < numOfEntities; i++)
            {
                int index = newClustering[i];
                ++clusters[index];
            }

            for (int i = 0; i < numOfClusters; i++)
                if (clusters[i] == 0)
                    return false;

            clustering = newClustering;

            return true;
        }

        private List<int> InitializationClustering(int numOfEntities, int numOfClusters, int seed)
        {
            List<int> clustering = new List<int>(numOfEntities);
            Random random        = new Random(seed);

            for (int i = 0; i < numOfClusters; i++)
                clustering.Add(i);

            for (int i = numOfClusters; i < numOfEntities; i++)
                clustering.Add(random.Next(0, numOfClusters));
        
            return clustering;
        }

        private List<IEntity> InitializationMeans(int numOfClusters, List<IEntity> entities)
        {
            List<IEntity> means = new List<IEntity>(numOfClusters);

            for (int i = 0; i < numOfClusters; i++)
                means.Add(entities[i]);

            return means;
        }

        private List<List<IEntity>> GetClusters(List<IEntity> entities, List<int> clustering, int numOfClusters)
        {
            List<List<IEntity>> resultClusters = new List<List<IEntity>>(numOfClusters);
            int count                          = clustering.Count;

            for (int i = 0; i < numOfClusters; i++)
                resultClusters.Add(new List<IEntity>());

            for (int i = 0; i < count; i++)
                resultClusters[clustering[i]].Add(entities[i]);

            return resultClusters;
        }

        private List<List<IEntity>> GetClusters(List<IEntity> entities, int numOfClusters)
        {
            List<List<IEntity>> resultClusters = new List<List<IEntity>>(numOfClusters);

            for (int i = 0; i < numOfClusters; i++)
                resultClusters.Add(new List<IEntity>());

            for (int i = 0; i < numOfClusters; i++)
                resultClusters[i].Add(entities[i]);

            return resultClusters;
        }


        private int GetMinIndex(List<double> distance)
        {
            int numOfDistances = distance.Count;
            int minIndex       = 0;
            double minDistance = distance[minIndex];

            for (int i = 0; i < numOfDistances; i++)
                if (distance[i] < minDistance)
                {
                    minDistance = distance[i];
                    minIndex = i;
                }

            return minIndex;
        }
    }
}