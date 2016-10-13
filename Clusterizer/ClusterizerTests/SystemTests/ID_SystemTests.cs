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
        private const string firstInputFile = "in1.txt";
        private const string secondInputFile = "in2.txt";
        private const string outputFile = "out.txt";

        [TestInitialize]
        public void Initialize()
        {
            GenerateTestInputFiles(firstInputFile, secondInputFile);
        }

        [TestMethod]
        public void TestTwoFilesClusterizeThroughDb()
        {
            // arrange
            List<List<string>> clusters = null;

            // act
            Clusterizer.Program.Main(new string[] { "-clr" });
            Clusterizer.Program.Main(new string[] { "-add", firstInputFile });
            Clusterizer.Program.Main(new string[] { "-add", secondInputFile });
            Clusterizer.Program.Main(new string[] { "-db", outputFile, "3" });
            Clusterizer.Program.Main(new string[] { "-clr" });
            using (var f = new StreamReader(outputFile))
            {
                var file = f.ReadToEnd();
                clusters = file.Split('@').Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Split('#').ToList()).ToList();
            }

            // assert
            Assert.IsTrue(clusters.Count == 3);
            Assert.IsTrue(clusters.Select(l => l.Count).Count(c => c == 0) == 0);
        }

        [TestMethod]
        public void TestOneFileClusterize()
        {
            // arrange
            List<List<string>> clusters = null;

            // act
            Clusterizer.Program.Main(new string[] { "-f", firstInputFile, outputFile, "2" });
            using (var f = new StreamReader(outputFile))
            {
                var file = f.ReadToEnd();
                clusters = file.Split('@').Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Split('#').ToList()).ToList();
            }

            // assert
            Assert.IsTrue(clusters.Count == 2);
            Assert.IsTrue(clusters.Select(l => l.Count).Count(c => c == 0) == 0);
        }

        [TestMethod]
        public void TestEmptyDbClusterize()
        {
            // arrange
            MemoryStream memory = new MemoryStream();
            TextWriter writer = new StreamWriter(memory);
            TextReader reader = new StreamReader(memory);

            // act
            Console.SetOut(writer);
            Clusterizer.Program.Main(new string[] { "-clr" });
            Clusterizer.Program.Main(new string[] { "-db", outputFile, "3" });
            writer.Flush();
            memory.Position = 0;

            // assert
            Assert.IsTrue(reader.ReadToEnd().Contains("Ошибка кластеризации"));
        }

        private void GenerateTestInputFiles(string filename1, string filename2)
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
