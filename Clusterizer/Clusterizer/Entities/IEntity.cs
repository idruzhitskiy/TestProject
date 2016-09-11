using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clusterizer.Entities
{
    /// <summary>
    /// Кластеризуемая сущность
    /// </summary>
    public interface IEntity
    {
        List<List<string>> TextAttributes { get; }
    }
}
