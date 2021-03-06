﻿using Clusterizer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clusterizer.DistanceFunctions
{
    /// <summary>
    /// Функция расстояния
    /// </summary>
    public interface IDistanceFunction
    {
        /// <summary>
        /// Функция расстояния. 
        /// Нормализована, принимает значения от 0 до 1, где 0 - полное совпадение, 1 - наибольшее расстояние.
        /// В случае несравнимости или расстояния, равного бесконечности - возвращает -1
        /// </summary>
        /// <param name="entity1">Сущность</param>
        /// <param name="entity2">Сущность</param>
        /// <returns>{-1} u [0;1]</returns>
        double Distance(IEntity entity1, IEntity entity2);

        /// <summary>
        /// Поиск центроиды
        /// </summary>
        /// <param name="entities">Сущности</param>
        /// <returns>Центральная сущность</returns>
        IEntity Centroid(List<IEntity> entities);
    }
}
