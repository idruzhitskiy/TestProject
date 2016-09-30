using System;
using Moq;
using Ninject;
using Clusterizer.Entities;
using System.Collections.Generic;
using System;
using System.Linq;
using Clusterizer.EntitiesWriters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClusterizerTests.WriterTests
{
    [TestClass]
    public class EntitiesWriterTests : BaseTest
    {
        [TestMethod]
        public void EmptyListOfClusters()
        {
            //arrange             
            var clusters = new List<List<IEntity>> { };

            var writer = new EntitiesWriter(new System.IO.StringWriter());
            writer.Write(clusters);
            
            //sets
        }

        [TestMethod]
        public void FullListOfClusters()
        {
            // arrange
            var clusters = new List<List<IEntity>> { new List<IEntity> { CreateEntity(new[] { new[] { "a1" } }),
                                                                         CreateEntity(new[] { new[] { "a2" } })  },
                                                     new List<IEntity> { CreateEntity(new[] { new[] { "b1", "b2"} }) } };
            var writer = new EntitiesWriter(new System.IO.StringWriter());
            writer.Write(clusters);

            //sets           
        }

        private IEntity CreateEntity(IEnumerable<IEnumerable<string>> attrs)
        {
            return Mock.Of<IEntity>(e => e.TextAttributes == new List<List<string>>(attrs.Select(el => new List<string>(el))));
        }
    }
}
