using Clusterizer.DistanceFunction;
using Clusterizer.EntitiesReader;
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
        public void MinMaxDistanceTest ()
        {
            // arrange
            var readerMock = new Mock<IEntitiesReader>();
            readerMock.Setup(r => r.Entities).Returns(new List<IEntity>
            {
                Mock.Of<IEntity>(e => e.TextAttributes == new List<List<string>> {new List<string> { "1","2","3"} }),
                Mock.Of<IEntity>(e => e.TextAttributes == new List<List<string>> {new List<string> { "4","5","6"} })
            });
            var reader = readerMock.Object;
            kernel.Rebind<IEntitiesReader>().ToConstant(reader);
            var distanceFunction = kernel.Get<IDistanceFunction>();

            // act
            var maxDistance = distanceFunction.Distance(reader.Entities[0], reader.Entities[1]);
            var minDistance = distanceFunction.Distance(reader.Entities[0], reader.Entities[0]);

            // assert
            Assert.IsTrue(maxDistance == 1);
            Assert.IsTrue(minDistance == 0);
        }
    }
}
