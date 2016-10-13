using Clusterizer.EntitiesReaders;
using Clusterizer.RemoteDatabases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClusterizerTests.FunctionalTests
{
    [TestClass]
    public class ID_FunctionalTests : BaseTest
    {
        private const string inputFile = "in1.txt";

        [TestMethod]
        public void TestDbAdd()
        {
            // arrange
            GenerateTestFile(inputFile);
            kernel.Rebind<TextReader>().ToConstant(new StreamReader(inputFile));
            var entities = kernel.Get<IEntitiesReader>().Entities;

            // act
            Clusterizer.Program.Main(new string[] { "-clr" });
            Clusterizer.Program.Main(new string[] { "-add", inputFile });
            var entitiesFromDb = kernel.Get<IRemoteDatabase>().Entities;

            // assert
            foreach (var entity in entities)
                Assert.IsTrue(entitiesFromDb
                    .Select(e => e.TextAttributes.SelectMany(l => l).Aggregate(string.Concat))
                    .Contains(entity.TextAttributes.SelectMany(l => l).Aggregate(string.Concat)));
        }

        public void TestDbRemove()
        {
            // arrange
            GenerateTestFile(inputFile);
            kernel.Rebind<TextReader>().ToConstant(new StreamReader(inputFile));
            var entities = kernel.Get<IEntitiesReader>().Entities;

            // act
            Clusterizer.Program.Main(new string[] { "-clr" });
            Clusterizer.Program.Main(new string[] { "-add", inputFile });
            Clusterizer.Program.Main(new string[] { "-rm", inputFile });
            var entitiesFromDb = kernel.Get<IRemoteDatabase>().Entities;

            // assert
            Assert.IsTrue(entities.Count > 0 && entitiesFromDb.Count < 0);
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
