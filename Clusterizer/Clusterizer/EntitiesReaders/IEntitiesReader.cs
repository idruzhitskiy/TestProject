using Clusterizer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clusterizer.EntitiesReaders
{
    /// <summary>
    /// Интерфейс получения сущностей
    /// </summary>
    public interface IEntitiesReader
    {
        List<IEntity> Entities { get; }
    }
}
