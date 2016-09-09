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
    public class Entity
    {
        private List<List<string>> textAttributes;

        public Entity(IEnumerable<IEnumerable<string>> textAttributes)
        {
            this.textAttributes = new List<List<string>>(textAttributes.Select(e => new List<string>(e)));
        }

        public List<List<string>> TextAttributes {
            get
            {
                return textAttributes;
            }
        }
    }
}
