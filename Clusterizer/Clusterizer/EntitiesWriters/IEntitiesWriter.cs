using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clusterizer.Entities;

namespace Clusterizer.EntitiesWriters
{
    public interface IEntitiesWriter
    {
        void Write(List<List<IEntity>> clusters);
    }    
}
