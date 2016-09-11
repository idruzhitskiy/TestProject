using Clusterizer.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClusterizerTests.EntitiesTests
{
    [TestClass]
    public class EntitiesFactoryTests : BaseTest
    {
        [TestMethod]
        public void TestFactory()
        {
            // arrange
            var attributes = new List<List<string>> { new List<string> { "Test", "attribute" } };
            var factory = kernel.Get<IEntitiesFactory>();

            // act
            var entity = factory.CreateEntity(attributes);

            // assert
            for (int i = 0; i < attributes[0].Count(); i++)
            {
                Assert.IsTrue(attributes[0][i] == entity.TextAttributes[0][i]);
            }
        }
    }
}
