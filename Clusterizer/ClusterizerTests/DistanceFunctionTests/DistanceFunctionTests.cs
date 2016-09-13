﻿using Clusterizer.DistanceFunctions;
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
using Clusterizer;

namespace ClusterizerTests
{
    [TestClass]
    public class DistanceFunctionTests : BaseTest
    {
        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();
            InitializeSimpleFactory();
        }

        [TestMethod]
        public void ArgumentsTest()
        {
            // arrange
            var entities = new List<IEntity>
            {
                Mock.Of<IEntity>(e => e.TextAttributes == new List<List<string>> {new List<string> { "1","1","1","1"}, new List<string> { "3" } }),
                Mock.Of<IEntity>(e => e.TextAttributes == new List<List<string>> {new List<string> { "2","2","2","2"} })
            };
            var reader = InitializeReader(new List<IEntity>());
            var distanceFunction = kernel.Get<IDistanceFunction>();
            bool distanceArgumentsException = false;
            bool centroidArgumentsException = false;

            // act
            try
            {
                distanceFunction.Distance(entities[0], entities[1]);
            }
            catch(ArgumentException)
            {
                distanceArgumentsException = true;
            }
            try
            {
                distanceFunction.Centroid(entities);
            }
            catch(ArgumentException)
            {
                centroidArgumentsException = true;
            }

            // assert
            Assert.IsTrue(distanceArgumentsException);
            Assert.IsTrue(centroidArgumentsException);
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

        [TestMethod]
        public void TestCentroid()
        {
            // arrange
            var entities = new List<IEntity>
            {
                Mock.Of<IEntity>(e => e.TextAttributes == new List<List<string>> {new List<string> { "0","0","0","0"} }),
                Mock.Of<IEntity>(e => e.TextAttributes == new List<List<string>> {new List<string> { "1","1","2","2"} })
            };
            var reader = InitializeReader(entities);
            var distanceFunction = kernel.Get<IDistanceFunction>();

            // act
            var centroid = distanceFunction.Centroid(reader.Entities);
            var distanceBetween = distanceFunction.Distance(reader.Entities[0], reader.Entities[1]);
            var distanceCentroidToFirst = distanceFunction.Distance(reader.Entities[0], centroid);
            var distanceCentroidToSecond = distanceFunction.Distance(reader.Entities[1], centroid);

            // assert
            Assert.IsTrue(distanceCentroidToFirst < distanceBetween);
            Assert.IsTrue(distanceCentroidToSecond < distanceBetween);
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
