using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clusterizer.Entities
{
    public class SimpleEntity : IEntity
    {
        private readonly List<List<string>> textAttributes;
        private readonly string id;

        public SimpleEntity(List<List<string>> attributes, string id = null)
        {
            this.textAttributes = attributes;
            this.id = id ?? Guid.NewGuid().ToString();
        }

        public string Id
        {
            get
            {
                return id;
            }
        }

        public List<List<string>> TextAttributes
        {
            get
            {
                return textAttributes;
            }
        }
    }
}
