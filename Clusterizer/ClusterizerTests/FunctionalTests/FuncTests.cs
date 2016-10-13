using Clusterizer.Clusterizers;
using Clusterizer.DistanceFunctions;
using Clusterizer.Entities;
using Clusterizer.EntitiesWriters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ClusterizerTests.ClusterizerTests;

// функциональные тесты 
// проверка функционала; программа представляется в виде черного ящика

namespace ClusterizerTests.FunctionalTests
{
    [TestClass]
    public class FuncTests : BaseTest
    {
        [TestMethod]
        // функциональный тест. проверяет работу кластеризатора  
        public void ClusterizeTest()
        {            
            InitializeDistanceFunction(
               (e1, e2) => Math.Abs(e1.TextAttributes[0].Count - e2.TextAttributes[0].Count) / (double)(e1.TextAttributes[0].Count + e2.TextAttributes[0].Count),
               l => CreateEntity(new List<List<string>>
               {
                    l.SelectMany(e => e.TextAttributes[0]).Take(l.SelectMany(e => e.TextAttributes[0]).Count() / l.Count).ToList()
               }));
            var clusterizer = kernel.Get<IClusterizer>();
            var elements = new List<IEntity> {
                CreateEntity(new[] { new[] {"слово1"} }),
                CreateEntity(new[] { new[] { "слово2", "cлово6"} }),
                CreateEntity(new[] { new[] { "слово3" } }),
                CreateEntity(new[] { new[] { "!", "!", "!", "!", "!", "!" } }),
                CreateEntity(new[] { new[] { "!", "!", "!", "!", "!" } })
            };

            // act
            var clusters = clusterizer.Clusterize(elements, 2);
            //kernel.Rebind<TextWriter>().ToConstant(new StreamWriter("out_filename.txt", false));
            //var entitiesWriter = kernel.Get<IEntitiesWriter>();
            //entitiesWriter.Write(clusters);

            // assert            
            int i = 0;          
            Assert.IsTrue(clusters[i].Contains(elements[0])
                && clusters[i].Contains(elements[1])
                && clusters[i].Contains(elements[2]));
            Assert.IsTrue(clusters[1 - i].Contains(elements[3])
                && clusters[1 - i].Contains(elements[4]) );

        }

        [TestMethod]
        public void WriterTest()
        {
            InitializeDistanceFunction(
              (e1, e2) => Math.Abs(e1.TextAttributes[0].Count - e2.TextAttributes[0].Count) / (double)(e1.TextAttributes[0].Count + e2.TextAttributes[0].Count),
              l => CreateEntity(new List<List<string>>
              {
                    l.SelectMany(e => e.TextAttributes[0]).Take(l.SelectMany(e => e.TextAttributes[0]).Count() / l.Count).ToList()
              }));
            var clusterizer = kernel.Get<IClusterizer>();
            var elements = new List<IEntity> {
                CreateEntity(new[] { new[] {"слово1"} }),
                CreateEntity(new[] { new[] { "слово2", "cлово6"} }),                
                CreateEntity(new[] { new[] { "!", "!", "!", "!", "!", "!" } }),
                CreateEntity(new[] { new[] { "!", "!", "!", "!", "!" } })
            };

            // act
            var clusters = clusterizer.Clusterize(elements, 2);
            var f = new StreamWriter("out_test.txt", false);
            kernel.Rebind<TextWriter>().ToConstant(f);
            var entitiesWriter = kernel.Get<IEntitiesWriter>();
            entitiesWriter.Write(clusters);

            TextReader reader = new StreamReader("out_test.txt");            

            // assert 
            Assert.IsTrue(reader.ReadLine() == "@Кластер 1");
            Assert.IsTrue(reader.ReadLine() == "	#сущность 1");
            Assert.IsTrue(reader.ReadLine() == "	-слово1 ");
            Assert.IsTrue(reader.ReadLine() == "	#сущность 2");
            Assert.IsTrue(reader.ReadLine() == "	-слово2 cлово6 ");
            Assert.IsTrue(reader.ReadLine() == "@Кластер 2");
            Assert.IsTrue(reader.ReadLine() == "	#сущность 1");
            Assert.IsTrue(reader.ReadLine() == "	-! ! ! ! ! ! ");
            Assert.IsTrue(reader.ReadLine() == "	#сущность 2");
            Assert.IsTrue(reader.ReadLine() == "	-! ! ! ! ! ");
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
