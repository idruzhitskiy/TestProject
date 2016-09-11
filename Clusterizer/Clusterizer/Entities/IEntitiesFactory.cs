using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clusterizer.Entities
{
    /// <summary>
    /// Фабрика сущностей
    /// </summary>
    public interface IEntitiesFactory
    {
        /// <summary>
        /// Создать сущность с указанными текстовыми атрибутами
        /// </summary>
        /// <param name="attrubutes">Атрибуты</param>
        /// <returns>Сущность</returns>
        IEntity CreateEntity(IEnumerable<IEnumerable<string>> attributes);
    }
}
