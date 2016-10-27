using Clusterizer.Clusterizers;
using Clusterizer.DistanceFunctions;
using Clusterizer.Entities;
using Clusterizer.EntitiesReaders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClusterizerTests.Version2Tests
{
    [TestClass]
    public class Tests : BaseTest
    {
        [TestMethod]
        public void TestBasicAccuracy()
        {
            // arrange
            var entities = new List<IEntity>
            {
                CreateEntity(new[] {new[] { "солнышко"} }),
                CreateEntity(new[] {new[] { "солнышки"} }),
                CreateEntity(new[] {new[] { "солнышке"} }),
                CreateEntity(new[] {new[] { "апельсин"} })

            };
            InitializeSimpleFactory();
            var reader = InitializeReader(entities);
            var distanceFunction = kernel.Get<IDistanceFunction>();
            var clusterizer = kernel.Get<IClusterizer>();

            // acr
            var clusters = clusterizer.Clusterize(reader.Entities, 2);

            // assert
            Assert.IsTrue(clusters.Find(l => l.Contains(entities[0])).Contains(entities[1]));
            Assert.IsTrue(clusters.Find(l => l.Contains(entities[0])).Contains(entities[2]));
            Assert.IsTrue(!clusters.Find(l => l.Contains(entities[0])).Contains(entities[3]));
        }

        [TestMethod]
        public void TestAdvancedAccuracyLongAttributes()
        {
            // arrange
            var entities = new List<IEntity>
            {
                CreateEntity(new[] {new[] { "кот","греется","на","солнышке"} }),
                CreateEntity(new[] {new[] { "кот","похож", "на", "шарик"} }),
                CreateEntity(new[] {new[] { "коты","похожи","на", "шарики"} })
            };
            InitializeSimpleFactory();
            var reader = InitializeReader(entities);
            var distanceFunction = kernel.Get<IDistanceFunction>();
            var clusterizer = kernel.Get<IClusterizer>();

            // acr
            var clusters = clusterizer.Clusterize(reader.Entities, 2);

            // assert
            Assert.IsTrue(!clusters.Find(l => l.Contains(entities[0])).Contains(entities[1]));
            Assert.IsTrue(!clusters.Find(l => l.Contains(entities[0])).Contains(entities[2]));
        }

        [TestMethod]
        public void TestAdvancedAccuracyLookalikeWords()
        {
            // arrange
            var entities = new List<IEntity>
            {
                CreateEntity(new[] {new[] { "солнце"} }),
                CreateEntity(new[] {new[] { "солнышко"} }),
                CreateEntity(new[] {new[] { "солнца"} })

            };
            InitializeSimpleFactory();
            var reader = InitializeReader(entities);
            var distanceFunction = kernel.Get<IDistanceFunction>();
            var clusterizer = kernel.Get<IClusterizer>();

            // acr
            var clusters = clusterizer.Clusterize(reader.Entities, 2);

            // assert
            Assert.IsTrue(!clusters.Find(l => l.Contains(entities[0])).Contains(entities[1]));
            Assert.IsTrue(clusters.Find(l => l.Contains(entities[0])).Contains(entities[2]));
        }

        private IEntity CreateEntity(IEnumerable<IEnumerable<string>> attrs)
        {
            return Mock.Of<IEntity>(e => e.TextAttributes == new List<List<string>>(attrs.Select(el => new List<string>(el))));
        }

        private IEntitiesReader InitializeReader(List<IEntity> entities)
        {
            var readerMock = new Mock<IEntitiesReader>();
            readerMock.Setup(r => r.Entities).Returns(entities);
            var reader = readerMock.Object;
            kernel.Rebind<IEntitiesReader>().ToConstant(reader);
            return reader;
        }

        private void InitializeSimpleFactory()
        {
            var factoryMock = new Mock<IEntitiesFactory>();
            factoryMock
                .Setup(r => r.CreateEntity(It.IsAny<List<List<string>>>()))
                .Returns<List<List<string>>>(attrs => Mock.Of<IEntity>(e => e.TextAttributes == attrs));
            kernel.Rebind<IEntitiesFactory>().ToConstant(factoryMock.Object);
        }
    }
}
