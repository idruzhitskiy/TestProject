using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clusterizer.Entities
{
    public class SimpleEntity : IEntity
    {
        private readonly List<List<string>> testAttributes;

        public SimpleEntity(List<List<string>> attributes)
        {
            this.testAttributes = attributes;
        }

        public List<List<string>> TextAttributes
        {
            get
            {
                return testAttributes;
            }
        }
    }
}
