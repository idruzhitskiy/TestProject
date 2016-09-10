using Clusterizer.DistanceFunctions;
using Clusterizer.EntitiesReaders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Clusterizer.Entities;
namespace ClusterizerTests
{
    [TestClass]
    public class DistanceFunctionTests
    {
        private IKernel kernel;

        [TestInitialize]
        public void Initialize()
        {
            kernel = new StandardKernel(new DIModule());
        }

        [TestMethod]
        public void MinMaxDistanceTest()
        {
            // arrange
            var entities = new List<IEntity>
            {
                Mock.Of<IEntity>(e => e.TextAttributes == new List<List<string>> {new List<string> { "1","1","1","1"} }),
                Mock.Of<IEntity>(e => e.TextAttributes == new List<List<string>> {new List<string> { "2","2","2","2"} })
            };
            var reader = InitializeReader(entities);
            var distanceFunction = kernel.Get<IDistanceFunction>();

            // act
            var maxDistance = distanceFunction.Distance(reader.Entities[0], reader.Entities[1]);
            var minDistance = distanceFunction.Distance(reader.Entities[0], reader.Entities[0]);

            // assert
            Assert.IsTrue(maxDistance == 1);
            Assert.IsTrue(minDistance == 0);
        }

        [TestMethod]
        public void TestDifferentDistances()
        {
            // arrange
            var entities = new List<IEntity>
            {
                Mock.Of<IEntity>(e => e.TextAttributes == new List<List<string>> {new List<string> { "0","0","0","0"} }),
                Mock.Of<IEntity>(e => e.TextAttributes == new List<List<string>> {new List<string> { "0","0","0","2"} }),
                Mock.Of<IEntity>(e => e.TextAttributes == new List<List<string>> {new List<string> { "0","0","2","3"} }),
                Mock.Of<IEntity>(e => e.TextAttributes == new List<List<string>> {new List<string> { "0","2","3","4"} }),
                Mock.Of<IEntity>(e => e.TextAttributes == new List<List<string>> {new List<string> { "1","2","3","4"} })
            };
            var reader = InitializeReader(entities);
            var distanceFunction = kernel.Get<IDistanceFunction>();

            // act
            var oneQuarterDiff = distanceFunction.Distance(reader.Entities[0], reader.Entities[1]);
            var twoQuartersDiff = distanceFunction.Distance(reader.Entities[0], reader.Entities[2]);
            var threeQuartersDiff = distanceFunction.Distance(reader.Entities[0], reader.Entities[3]);
            var fullDiff = distanceFunction.Distance(reader.Entities[0], reader.Entities[4]);

            // assert
            Assert.IsTrue(oneQuarterDiff < twoQuartersDiff);
            Assert.IsTrue(twoQuartersDiff < threeQuartersDiff);
            Assert.IsTrue(threeQuartersDiff < fullDiff);
        }

        private IEntitiesReader InitializeReader(List<IEntity> entities)
        {
            var readerMock = new Mock<IEntitiesReader>();
            readerMock.Setup(r => r.Entities).Returns(entities);
            var reader = readerMock.Object;
            kernel.Rebind<IEntitiesReader>().ToConstant(reader);
            return reader;
        }
    }
}
