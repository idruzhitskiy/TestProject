using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClusterizerTests.SystemTests
{
    [TestClass]
    public class ID_SystemTests
    {
        [TestMethod]
        public void Test1()
        {
            GenerateTestInputFile("in1.txt", "in2.txt");
            Clusterizer.Program.Main(new string[] { "-add", "in1.txt" });
            Clusterizer.Program.Main(new string[] { "-add", "in2.txt" });
            Clusterizer.Program.Main(new string[] { "-db", "out.txt", "4" });
            Clusterizer.Program.Main(new string[] { "-clr" });
        }


        private void GenerateTestInputFile(string filename1 = "in1.txt", string filename2 = "in2.txt")
        {
            using (var f = new StreamWriter(filename1, false))
            {
                f.WriteLine("#сущность");
                f.WriteLine("-над Москвой было. ");
                f.WriteLine("-пасмурное небо. ");
                f.WriteLine("#сущность");
                f.WriteLine("-в Москве было. ");
                f.WriteLine("-холодно. ");
                f.WriteLine("#сущность ");
                f.WriteLine("-над Москвой летали. ");
                f.WriteLine("-самолеты.");
                f.WriteLine("#сущность");
                f.WriteLine("-погоду в Москве обещали. ");
                f.WriteLine("-хорошую.");
            }

            using (var f = new StreamWriter(filename2, false))
            {
                f.WriteLine("#сущность");
                f.WriteLine("-над Питером было. ");
                f.WriteLine("-безоблачное небо. ");
                f.WriteLine("#сущность");
                f.WriteLine("-в Питере было. ");
                f.WriteLine("-тепло. ");
                f.WriteLine("#сущность");
                f.WriteLine("-погоду в Питере обещали. ");
                f.WriteLine("-хорошую.");
            }
        }
    }
}
