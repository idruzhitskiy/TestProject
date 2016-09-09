using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Clusterizer.DistanceFunction;

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
    }
}
