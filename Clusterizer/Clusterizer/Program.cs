using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Clusterizer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Count() < 3)
            {
                Console.WriteLine("Использование: clusterizer.exe filename num_of_clusters");
            }


            // Инициализация DI
            IKernel kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());

        }
    }
}
