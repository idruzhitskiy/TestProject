using Clusterizer.DistanceFunctions;
using Clusterizer.EntitiesReaders;
using Clusterizer.Entities;
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
            Bind<IEntitiesReader>().To<EntitiesReader>();
            Bind<IEntitiesFactory>().To<SimpleEntitiesFactory>();
        }
    }
}
