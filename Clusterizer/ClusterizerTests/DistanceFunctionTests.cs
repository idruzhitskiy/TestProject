using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Clusterizer.DistanceFunction;
using Clusterizer.Entities;
using System.Linq;

namespace ClusterizerTests
{
    [TestClass]
    public class DistanceFunctionTests
    {
        [TestMethod]
        public void ConstructorTest()
        {
            try
            {
                var functionEmptyList = new DistanceFunction(new List<List<string>>());
                var functionNormalList = new DistanceFunction(new List<List<string>>
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
            var entities = new List<Entity>
            {
                new Entity(new List<List<string>>
                {
                    new List<string> {"кот", "с", "черными", "ушами"}
                }),
                new Entity(new List<List<string>>
                {
                    new List<string> {"собака", "с", "черными", "ушами"}
                }),
                new Entity(new List<List<string>>
                {
                    new List<string> {"собака", "с", "рыжим", "хвостом"}
                }),
                new Entity(new List<List<string>>
                {
                    new List<string> {"собака", "с", "рыжим", "хвостом", "и", "длинными", "ушами"}
                }),
                new Entity(new List<List<string>>
                {
                    new List<string> {"собака", "с", "рыжим", "хвостом", "и", "длинными", "ушами", "и", "голубыми", "глазами"}
                }),
            };
            var textAttributes = entities.SelectMany(e => e.TextAttributes).ToList();
            var function = new DistanceFunction(textAttributes);
            var resultIdentical = function.Distance(entities[0], entities[0]);
            var resultAboveMiddle = function.Distance(entities[0], entities[1]);
            var resultBelowMiddle = function.Distance(entities[0], entities[2]);
            var resultSameStartSmallDifferentLength = function.Distance(entities[2], entities[3]);
            var resultSameStartBigDifferentLength = function.Distance(entities[2], entities[4]);
            // act

            // assert
            Assert.IsTrue(resultIdentical == 1);
            Assert.IsTrue(resultAboveMiddle > 0.5 && resultAboveMiddle < 1);
            Assert.IsTrue(resultBelowMiddle > 0 && resultBelowMiddle < 0.5);
            Assert.IsTrue(resultSameStartSmallDifferentLength > 0 &&
                resultSameStartBigDifferentLength < 1 &&
                resultSameStartBigDifferentLength < resultSameStartSmallDifferentLength);
        }
    }
}
