using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClusterizerTests.SystemTests
{
    [TestClass]
    public class ID_SystemTests
    {
        // test
        private string firstInputFile
        {
            get
            {
                return GetMethodName() + "_in1.txt";
            }
        }
        private string secondInputFile
        {
            get
            {
                return GetMethodName() + "_in2.txt";
            }
        }
        private string thirdInputFile
        {
            get
            {
                return GetMethodName() + "_in3.txt";
            }
        }

        private string outputFile
        {
            get
            {
                return GetMethodName() + "_out.txt";
            }
        }

        [TestMethod]
        public void TestNoArguments()
        {
            // arrange
            MemoryStream memory = new MemoryStream();
            TextWriter writer = new StreamWriter(memory);
            TextReader reader = new StreamReader(memory);

            // act
            Console.SetOut(writer);
            Clusterizer.Program.Main(new List<string>().ToArray());
            writer.Flush();
            memory.Position = 0;

            // assert
            Assert.IsTrue(reader.ReadToEnd().Contains("Использование:"));
        }

        [TestMethod]
        public void TestTwoFilesClusterizeThroughDb()
        {
            // arrange
            List<List<string>> clusters = null;
            GenerateTestInputFiles(firstInputFile, secondInputFile, thirdInputFile);

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
        public void TestNoCommand()
        {
            // arrange
            MemoryStream memory = new MemoryStream();
            TextWriter writer = new StreamWriter(memory);
            TextReader reader = new StreamReader(memory);

            // act
            Console.SetOut(writer);
            Clusterizer.Program.Main(new string[] { "" });
            writer.Flush();
            memory.Position = 0;

            // assert
            Assert.IsTrue(reader.ReadToEnd().Contains("Команда не найдена"));
        }

        [TestMethod]
        public void TestTooManyArguments()
        {
            // arrange
            MemoryStream memory = new MemoryStream();
            TextWriter writer = new StreamWriter(memory);
            TextReader reader = new StreamReader(memory);

            // act
            Console.SetOut(writer);
            Clusterizer.Program.Main(new string[] { "-f", "in.txt", outputFile, "2", "100" });
            writer.Flush();
            memory.Position = 0;

            // assert
            Assert.IsTrue(reader.ReadToEnd().Contains("Ошибка количества аргументов"));
        }

        [TestMethod]
        public void TestOneFileClusterize()
        {
            // arrange
            List<List<string>> clusters = null;
            GenerateTestInputFiles(firstInputFile, secondInputFile, thirdInputFile);

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
            Assert.IsTrue(clusters[0][1].Contains("над Москвой было"));
            Assert.IsTrue(
                clusters[1][1].Contains("в Москве было") &&
                clusters[1][2].Contains("над Москвой летали") &&
                clusters[1][3].Contains("погоду в Москве обещали")
                );
        }

        [TestMethod]
        public void TestClusterizer()
        {
            // arrange
            List<List<string>> clusters = null;
            GenerateTestInputFiles(firstInputFile, secondInputFile, thirdInputFile);

            // act
            Clusterizer.Program.Main(new string[] { "-f", thirdInputFile, outputFile, "2" });
            using (var f = new StreamReader(outputFile))
            {
                var file = f.ReadToEnd();
                clusters = file.Split('@').Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Split('#').ToList()).ToList();
            }

            // assert
            Assert.IsTrue(clusters.Count == 2);
            Assert.IsTrue(clusters.Select(l => l.Count).Count(c => c == 0) == 0);
            Assert.IsTrue(
                clusters[0][1].Contains("черный кот") &&
                clusters[0][2].Contains("черный код")
                );
            Assert.IsTrue(
                clusters[1][1].Contains("серая мышь")
                );
        }

        [TestMethod]
        public void TestSameNumberElementsAndClusters()
        {
            // arrange
            List<List<string>> clusters = null;
            GenerateTestInputFiles(firstInputFile, secondInputFile, thirdInputFile);

            // act
            Clusterizer.Program.Main(new string[] { "-f", secondInputFile, outputFile, "3" });
            using (var f = new StreamReader(outputFile))
            {
                var file = f.ReadToEnd();
                clusters = file.Split('@').Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Split('#').ToList()).ToList();
            }

            // assert
            Assert.IsTrue(clusters.Count == 3);
            Assert.IsTrue(clusters.Select(l => l.Count).Count(c => c == 0) == 0);
            Assert.IsTrue(clusters[0][1].Contains("над Питером было"));
            Assert.IsTrue(clusters[1][1].Contains("в Питере было"));
            Assert.IsTrue(clusters[2][1].Contains("погоду в Питере обещали"));
        }

        [TestMethod]
        public void TestNegativeNumberOfClusters()
        {
            // arrange
            MemoryStream memory = new MemoryStream();
            TextWriter writer = new StreamWriter(memory);
            TextReader reader = new StreamReader(memory);
            GenerateTestInputFiles(firstInputFile, secondInputFile, thirdInputFile);

            // act
            Console.SetOut(writer);
            Clusterizer.Program.Main(new string[] { "-f", firstInputFile, outputFile, "-5" });

            writer.Flush();
            memory.Position = 0;

            // assert
            Assert.IsTrue(reader.ReadToEnd().Contains("Ошибка кластеризации"));
        }

        [TestMethod]
        public void TestClusterNumberMoreThanEntitiesNumber()
        {
            // arrange
            MemoryStream memory = new MemoryStream();
            TextWriter writer = new StreamWriter(memory);
            TextReader reader = new StreamReader(memory);
            GenerateTestInputFiles(firstInputFile, secondInputFile, thirdInputFile);

            // act
            Console.SetOut(writer);
            Clusterizer.Program.Main(new string[] { "-f", secondInputFile, outputFile, "4" });

            writer.Flush();
            memory.Position = 0;

            // assert
            Assert.IsTrue(reader.ReadToEnd().Contains("Ошибка кластеризации"));
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

        private void GenerateTestInputFiles(string filename1, string filename2, string filename3)
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

            using (var f = new StreamWriter(filename3, false))
            {
                f.WriteLine("#сущность");
                f.WriteLine("-черный кот. ");
                f.WriteLine("#сущность");
                f.WriteLine("-серая мышь. ");
                f.WriteLine("#сущность");
                f.WriteLine("-черный код. ");
            }
        }

        private string GetMethodName()
        {
            var st = new StackTrace(new StackFrame(2));
            return st.GetFrame(0).GetMethod().Name;
        }
    }
}
