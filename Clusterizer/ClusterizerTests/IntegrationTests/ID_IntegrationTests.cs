using Clusterizer.EntitiesReaders;
using Clusterizer.EntitiesWriters;
using Clusterizer.RemoteDatabases;
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
    }
}
