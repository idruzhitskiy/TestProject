using Clusterizer.Clusterizers;
using Clusterizer.DistanceFunctions;
using Clusterizer.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClusterizerTests.ClusterizerTests
{
    [TestClass]
    public class ClusterizerTests : BaseTest
    {
        [TestMethod]
        public void SimpleTest()
        {
            // arrange
            InitializeDistanceFunction(
                (e1, e2) => 1,
                l => CreateEntity(new[] { new[] { "1", "2" } }));
            var clusterizer = kernel.Get<IClusterizer>();
            var oneElement = new List<IEntity> { CreateEntity(new[] { new[] { "1" } }) };
            var twoElements = new List<IEntity> { CreateEntity(new[] { new[] { "1" } }), CreateEntity(new[] { new[] { "2" } }) };
            bool exceptionOnZeroClusters = false;
            bool exceptionOnEmptyList = false;

            // act
            try
            {
                var temp = clusterizer.Clusterize(twoElements, 0);
            }
            catch
            {
                exceptionOnZeroClusters = true;
            }
            try
            {
                var temp = clusterizer.Clusterize(new List<IEntity>(), 2);
            }
            catch
            {
                exceptionOnEmptyList = true;
            }
            var oneElementOneCluster = clusterizer.Clusterize(oneElement, 1);
            var twoClusters = clusterizer.Clusterize(twoElements, 2);

            // assert
            Assert.IsTrue(exceptionOnZeroClusters);
            Assert.IsTrue(exceptionOnEmptyList);
            Assert.IsTrue(oneElementOneCluster.Count == 1);
            Assert.IsTrue(twoClusters.Count == 2);
        }

        private void InitializeDistanceFunction(Func<IEntity, IEntity, double> dist, Func<List<IEntity>, IEntity> centr)
        {
            var distanceFunctionMock = new Mock<IDistanceFunction>();
            distanceFunctionMock.Setup(f => f.Distance(It.IsAny<IEntity>(), It.IsAny<IEntity>())).Returns(dist);
            distanceFunctionMock.Setup(f => f.Centroid(It.IsAny<List<IEntity>>())).Returns(centr);
            kernel.Rebind<IDistanceFunction>().ToConstant(distanceFunctionMock.Object);
        }

        private IEntity CreateEntity(IEnumerable<IEnumerable<string>> attrs)
        {
            return Mock.Of<IEntity>(e => e.TextAttributes == new List<List<string>>(attrs.Select(el => new List<string>(el))));
        }
    }
}
