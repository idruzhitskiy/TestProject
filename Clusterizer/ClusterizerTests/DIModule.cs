using Clusterizer.DistanceFunctions;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClusterizerTests
{
    class DIModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDistanceFunction>().To<DistanceFunction>();
        }
    }
}
