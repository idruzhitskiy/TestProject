﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clusterizer.Entities
{
    class SimpleEntitiesFactory : IEntitiesFactory
    {
        public IEntity CreateEntity(List<List<string>> attributes)
        {
            return new SimpleEntity(attributes);
        }
    }
}
