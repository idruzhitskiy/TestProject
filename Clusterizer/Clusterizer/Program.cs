using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Clusterizer.Clusterizers;
using Clusterizer.EntitiesReaders;

namespace Clusterizer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Count() < 2)
            {
                Console.WriteLine("Использование: clusterizer.exe filename num_of_clusters");
                return;
            }

            // Инициализация DI
            IKernel kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            kernel.Rebind<TextReader>().ToConstant(new StreamReader(args[0]));
            var res = kernel.Get<IClusterizer>().Clusterize(kernel.Get<IEntitiesReader>().Entities, Convert.ToInt32(args[1]));
        }
    }
}
