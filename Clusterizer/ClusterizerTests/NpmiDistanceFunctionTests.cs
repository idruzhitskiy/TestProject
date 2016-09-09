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
            Entity entity1 = new Entity(new List<List<string>>
            {
                new List<string> {"кот"},
                new List<string> {"рыжее", "тело" },
            });
            Entity entity2 = new Entity(new List<List<string>>
            {
                new List<string> {"собака"},
                new List<string> {"черное", "тело" },
            });
            var function = new NpmiDistanceFunction(entity1.TextAttributes.Union(entity2.TextAttributes.Union(new List<List<string>> { new List<string> { "кот", "собака" } })));
            var result = function.Distance(entity1, entity2);
            Assert.IsTrue(result > 0 && result < 1);
        }
    }
}
