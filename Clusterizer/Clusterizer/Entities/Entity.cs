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
    class Entity
    {
        private List<string> textAttributes;

        public Entity(IEnumerable<string> textAttributes)
        {
            this.textAttributes = new List<string>(textAttributes);
        }

        public List<string> TextAttributes { get; }
    }
}
