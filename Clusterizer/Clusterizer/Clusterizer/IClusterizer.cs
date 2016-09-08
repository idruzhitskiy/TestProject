using Clusterizer.DistanceFunction;
using Clusterizer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clusterizer.Clusterizer
{
    /// <summary>
    /// Кластеризатор
    /// </summary>
    interface IClusterizer
    {
        /// <summary>
        /// Функция кластеризации
        /// </summary>
        /// <param name="distanceFunction">Функция расстояния</param>
        /// <param name="numOfClusters">Количество кластеров</param>
        /// <returns>Список, содержащий кластеры</returns>
        List<List<Entity>> Clusterize(IDistanceFunction distanceFunction, int numOfClusters);
    }
}
