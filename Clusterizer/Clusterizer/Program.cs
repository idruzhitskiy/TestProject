using Clusterizer.DistanceFunction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clusterizer
{
    class Program
    {
        static void Main(string[] args)
        {
            NpmiDistanceFunction f = new NpmiDistanceFunction(new List<List<string>>{
                new List<string> {"Иван", "родил", "девченку" },
                new List<string> {"Велит", "тащить", "пеленку" },
                new List<string> {"родил", "девченку" }
            });
        }
    }
}
