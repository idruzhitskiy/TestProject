using Clusterizer.EntitiesReaders;
using Clusterizer.EntitiesWriters;
using Clusterizer.RemoteDatabases;
using Clusterizer.Clusterizers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClusterizerTests.IntegrationTests
{
    [TestClass]
    public class ID_IntegrationTests : BaseTest
    {
        private const string inputFile = "in5.txt";

        [TestMethod]
        public void TestReaderDatabase()
        {
            // arrange
            kernel.Bind<TextReader>().ToConstant(GetStream());
            var reader = kernel.Get<IEntitiesReader>();
            var remoteDatabase = kernel.Get<IRemoteDatabase>();

            // act
            remoteDatabase.DropDatabase();
            foreach (var entity in reader.Entities)
            {
                remoteDatabase.AddEntity(entity);
            }

            // assert
            foreach (var entity in reader.Entities)
                Assert.IsTrue(remoteDatabase.Entities
                    .Select(e => e.TextAttributes.SelectMany(l => l).Aggregate(string.Concat))
                    .Contains(entity.TextAttributes.SelectMany(l => l).Aggregate(string.Concat)));
        }

        [TestMethod]
        public void TestReaderWriter()
        {
            // arrange
            var memoryStream = new MemoryStream();
            var outStream = new StreamWriter(memoryStream);
            kernel.Bind<TextReader>().ToConstant(GetStream());
            kernel.Bind<TextWriter>().ToConstant(outStream);
            var reader = kernel.Get<IEntitiesReader>();
            var writer = kernel.Get<IEntitiesWriter>();
            IEnumerable<string> resultEntitiesAttributes = null;

            // act
            writer.Write(new List<List<Clusterizer.Entities.IEntity>> { reader.Entities });
            using (var inStream = new StreamReader(new MemoryStream(memoryStream.ToArray())))
            {
                resultEntitiesAttributes = inStream.ReadToEnd().Split('@').Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Split('#').ToList()).First().Skip(1);
            }

            // assert
            foreach (var entity in reader.Entities)
            {
                foreach (var attr in entity.TextAttributes)
                {
                    foreach (var word in attr)
                    {
                        Assert.IsTrue(resultEntitiesAttributes.Any(s => s.Contains(word)));
                    }
                }
            }
        }

        [TestMethod]
        public void ClusterizatorReader()
        {
            GenerateTestFile(inputFile);
            kernel.Rebind<TextReader>().ToConstant(new StreamReader(inputFile));
            var elements = kernel.Get<IEntitiesReader>().Entities;
            var clusterizer = kernel.Get<IClusterizer>();          

            // act
            var clusters = clusterizer.Clusterize(elements, 2);

            // assert
            Assert.IsTrue(clusters.Count() == 2);
            Assert.IsTrue(clusters[0].Contains(elements[0]));
            Assert.IsTrue(clusters[1].Contains(elements[1])
                            && clusters[1].Contains(elements[2])
                            && clusters[1].Contains(elements[3]));

        }

        [TestMethod]
        public void ClusterizatorWriter()
        {
            // arrange
            GenerateTestFile(inputFile);
            kernel.Rebind<TextReader>().ToConstant(new StreamReader(inputFile));

            // act
            Clusterizer.Program.Main(new string[] { "-f", inputFile, "out_test1.txt", "2" });
            TextReader reader = new StreamReader("out_test1.txt");

            // assert 
            Assert.IsTrue(reader.ReadLine() == "@Кластер 1");
            Assert.IsTrue(reader.ReadLine() == "	#сущность 1");
            Assert.IsTrue(reader.ReadLine() == "	-над Москвой было. ");
            Assert.IsTrue(reader.ReadLine() == "	-пасмурное небо. ");
        }


        private StreamReader GetStream()
        {
            var memory = new MemoryStream();
            var f = new StreamWriter(memory);
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
            f.Flush();
            memory.Position = 0;
            return new StreamReader(memory);
        }

        private void GenerateTestFile(string filename)
        {
            using (var f = new StreamWriter(filename, false))
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
        }
    }
}
