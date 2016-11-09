using Clusterizer.Clusterizers;
using Clusterizer.Entities;
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

namespace ClusterizerTests.AutomationTests
{
    [TestClass]
    public class AutomationTests : BaseTest
    {
        [TestMethod]
        [ClusterizerTest("1 Черный кот \n рыжая шерсть", "2 Белый лис")]
        [ClusterizerTest("1 кот греется на солнышке",
            "2 кот похож на шарик",
            "2 коты похожи на шарики")]
        [ClusterizerTest("1 солнышко", "1 солнышки", "1 солнышке", "2 апельсин")]
        public void ClusterizerAutoTest()
        {
            var attrs = (IEnumerable<ClusterizerTestAttribute>)MethodBase.GetCurrentMethod().GetCustomAttributes(typeof(ClusterizerTestAttribute));
            foreach (var attr in attrs)
                RunClusterizerTest(attr.Entities, attr.NumOfClusters);
        }

        private void RunClusterizerTest(List<Tuple<int, List<List<string>>>> entities, int numOfClusters)
        {
            // arrange
            var entititesDictionary = entities.ToDictionary(t => kernel.Get<IEntitiesFactory>().CreateEntity(t.Item2), t => t.Item1);
            var entititesReaderMock = new Mock<IEntitiesReader>();
            entititesReaderMock.Setup(e => e.Entities).Returns(entititesDictionary.Keys.ToList());
            kernel.Rebind<IEntitiesReader>().ToConstant(entititesReaderMock.Object);

            // act
            var clusters = kernel.Get<IClusterizer>().Clusterize(entititesDictionary.Keys.ToList(), numOfClusters);

            // assert
            foreach (var cluster in clusters)
            {
                var key = entititesDictionary[cluster[0]];
                foreach (var entity in cluster)
                {
                    Assert.AreEqual(key, entititesDictionary[entity]);
                }
            }
        }
    }
}
