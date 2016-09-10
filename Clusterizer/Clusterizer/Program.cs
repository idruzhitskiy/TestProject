using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Clusterizer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Инициализация DI
            IKernel kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
        }
    }
}
