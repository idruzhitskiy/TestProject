using Clusterizer.Entities;
using Clusterizer.EntitiesReaders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ninject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClusterizerTests.EntitiesReaderTests
{
    [TestClass]
    public class EntitiesReaderTests : BaseTest
    {
        [TestMethod]
        public void SingleEntityMultipleAttributes()
        {
            // arrange
            GenerateStreamFromString(
                            "#Entity\n" +
                            "- attribute number one\n" +
                            "- attriubte number two\n"
                            );
            InitializeSimpleEntitiesFactory();
            var reader = kernel.Get<IEntitiesReader>();

            // act
            var entities = reader.Entities;

            // assert
            Assert.IsTrue(entities.Count == 1);
            Assert.IsTrue(entities[0].TextAttributes[1][1] == "number");
        }

        [TestMethod]
        public void MultipleEntitiesMultipleAttributes()
        {
            // arrange
            GenerateStreamFromString(
                            "#Entity\n" +
                            "- attribute number one\n" +
                            "- attriubte number two\n" +
                            "#Entity\n" +
                            "- attribute number one\n" +
                            "- attriubte number two\n" +
                            "#Entity\n" +
                            "- attribute number one\n" +
                            "- attriubte number two\n"
                            );
            InitializeSimpleEntitiesFactory();
            var reader = kernel.Get<IEntitiesReader>();

            // act
            var entities = reader.Entities;

            // assert
            Assert.IsTrue(entities.Count == 3);
            Assert.IsTrue(entities[1].TextAttributes[1][2] == "two");
        }

        [TestMethod]
        public void MultipleEntitiesSingleAttribute()
        {
            // arrange
            GenerateStreamFromString(
                            "#Entity\n" +
                            "- attribute number one\n" +
                            "#Entity\n" +
                            "- attribute number one\n" +
                            "#Entity\n" +
                            "- attribute number one\n" +
                            "#Entity\n" +
                            "- attribute number one\n"
                            );
            InitializeSimpleEntitiesFactory();
            var reader = kernel.Get<IEntitiesReader>();

            // act
            var entities = reader.Entities;

            // assert
            Assert.IsTrue(entities.Count == 4);
            Assert.IsTrue(entities[2].TextAttributes[0][2] == "one");
        }

        private void InitializeSimpleEntitiesFactory()
        {
            var factoryMock = new Mock<IEntitiesFactory>();
            factoryMock.Setup(f => f.CreateEntity(It.IsAny<List<List<string>>>()))
                .Returns<List<List<string>>>(l => Mock.Of<IEntity>(e => e.TextAttributes == l));
            kernel.Rebind<IEntitiesFactory>().ToConstant(factoryMock.Object);
        }

        private void GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            kernel.Rebind<TextReader>().ToConstant(new StreamReader(stream));
        }
    }
}
