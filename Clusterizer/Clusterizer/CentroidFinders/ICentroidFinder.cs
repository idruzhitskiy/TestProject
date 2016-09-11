using Clusterizer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clusterizer.CentroidFinders
{
    /// <summary>
    /// Осуществляет поиск центроиды
    /// </summary>
    public interface ICentroidFinder
    {
        /// <summary>
        /// Поиск центроиды
        /// </summary>
        /// <param name="entities">Сущности</param>
        /// <returns>Центральная сущность</returns>
        IEntity Centroid(List<IEntity> entities);
    }
}
