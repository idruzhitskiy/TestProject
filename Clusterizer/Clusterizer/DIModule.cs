using Clusterizer.Clusterizers;
using Clusterizer.DistanceFunctions;
using Clusterizer.EntitiesReaders;
using Clusterizer.Entities;
using Clusterizer.EntitiesWriters;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clusterizer
{
    public class DIModule : NinjectModule
    {
        public override void Load()
        {
            // Сюда добавлять реализации абстрактных классов, например
            // Bind<Interface>().To<Realisation>()
            Bind<IDistanceFunction>().To<DistanceFunction>();
            Bind<IEntitiesReader>().To<EntitiesReader>().InSingletonScope();
            Bind<IEntitiesFactory>().To<SimpleEntitiesFactory>();
            Bind<IClusterizer>().To<Clusterizers.Clusterizer>();
            Bind<IEntitiesWriter>().To<EntitiesWriter>();
        }
    }
}
