using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Clusterizer.DistanceFunction;
using Clusterizer.Entities;
using System.Linq;

namespace ClusterizerTests
{
    /// <summary>
    /// Сводное описание для NpmiDistanceFunctionTests
    /// </summary>
    [TestClass]
    public class NpmiDistanceFunctionTests
    {
        [TestMethod]
        public void ConstructorTest()
        {
            try
            {
                var functionEmptyList = new NpmiDistanceFunction(new List<List<string>>());
                var functionNormalList = new NpmiDistanceFunction(new List<List<string>>
                {
                    new List<string> { "abc", "def" },
                    new List<string> {"ghi" }
                });
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void DistanceTest()
        {
            // arrange
            Entity entity1 = new Entity(new List<List<string>>
            {
                new List<string> {"кот", "с", "черными", "ушами"}
            });
            Entity entity2 = new Entity(new List<List<string>>
            {
                new List<string> {"собака", "с", "черными", "ушами"}
            });
            Entity entity3 = new Entity(new List<List<string>>
            {
                new List<string> {"собака", "с", "рыжим", "хвостом"}
            });
            var function = new NpmiDistanceFunction(entity1.TextAttributes.Union(entity2.TextAttributes).Union(entity3.TextAttributes));
            var resultIdentical = function.Distance(entity1, entity1);
            var resultAboveMiddle = function.Distance(entity1, entity2);
            var resultBelowMiddle = function.Distance(entity1, entity3);

            // act

            // assert
            Assert.IsTrue(resultIdentical == 1);
            Assert.IsTrue(resultAboveMiddle > 0.5 && resultAboveMiddle < 1);
            Assert.IsTrue(resultBelowMiddle > 0 && resultBelowMiddle < 0.5);
        }
    }
}
