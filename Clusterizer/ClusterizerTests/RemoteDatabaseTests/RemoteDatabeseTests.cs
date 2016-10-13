using Clusterizer.Entities;
using Clusterizer.RemoteDatabases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClusterizerTests.RemoteDatabaseTests
{
    [TestClass]
    public class RemoteDatabeseTests : BaseTest
    {
        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();
            InitializeSimpleEntitiesFactory();
        }

        [TestMethod]
        public void BaseTest()
        {
            // arrange
            var remoteDatabase = kernel.Get<IRemoteDatabase>();
            var factory = kernel.Get<IEntitiesFactory>();

            // act
            remoteDatabase.AddEntity(factory.CreateEntity(new List<List<string>> { new List<string> { "1", "2" } }));
            var entity = remoteDatabase.FindEntity(new List<List<string>> { new List<string> { "1", "2" } });
            remoteDatabase.RemoveEntity(entity);
            var nullEntity = remoteDatabase.FindEntity(new List<List<string>> { new List<string> { "1", "2" } });

            // assert
            Assert.IsNotNull(entity);
            Assert.IsNull(nullEntity);
        }

        [TestMethod]
        public void FindAllTest()
        {
            // arrange
            var remoteDatabase = kernel.Get<IRemoteDatabase>();
            var factory = kernel.Get<IEntitiesFactory>();
            var entity1 = factory.CreateEntity(new List<List<string>> { new List<string> { "1", "2" } });
            var entity2 = factory.CreateEntity(new List<List<string>> { new List<string> { "3", "4" } });
            var entity3 = factory.CreateEntity(new List<List<string>> { new List<string> { "5", "6" } });

            // act
            remoteDatabase.AddEntity(entity1);
            remoteDatabase.AddEntity(entity2);
            remoteDatabase.AddEntity(entity3);
            var entitiesInDatabase = remoteDatabase.FindAllEntitites();

            // assert
            Assert.IsTrue(entitiesInDatabase.Select(e => e.Id).Contains(entity1.Id));
            Assert.IsTrue(entitiesInDatabase.Select(e => e.Id).Contains(entity2.Id));
            Assert.IsTrue(entitiesInDatabase.Select(e => e.Id).Contains(entity3.Id));
        }

        [TestMethod]
        public void DropDatabaseTest()
        {
            // arrange
            var remoteDatabase = kernel.Get<IRemoteDatabase>();
            var factory = kernel.Get<IEntitiesFactory>();
            var entity1 = factory.CreateEntity(new List<List<string>> { new List<string> { "1", "2" } });
            var entity2 = factory.CreateEntity(new List<List<string>> { new List<string> { "3", "4" } });
            var entity3 = factory.CreateEntity(new List<List<string>> { new List<string> { "5", "6" } });

            // act
            remoteDatabase.AddEntity(entity1);
            remoteDatabase.AddEntity(entity2);
            remoteDatabase.AddEntity(entity3);
            remoteDatabase.DropDatabase();
            var emptyEntitiesList = remoteDatabase.FindAllEntitites();

            // assert
            Assert.IsTrue(emptyEntitiesList.Count == 0);
        }

        private void InitializeSimpleEntitiesFactory()
        {
            var factoryMock = new Mock<IEntitiesFactory>();
            factoryMock.Setup(f => f.CreateEntity(It.IsAny<List<List<string>>>()))
                .Returns<List<List<string>>>(l => Mock.Of<IEntity>(e => e.TextAttributes == l && e.Id == Guid.NewGuid().ToString()));
            factoryMock.Setup(f => f.CreateEntityWithId(It.IsAny<List<List<string>>>(), It.IsAny<string>()))
                .Returns<List<List<string>>, string>((l, id) => Mock.Of<IEntity>(e => e.TextAttributes == l && e.Id == id));
            kernel.Rebind<IEntitiesFactory>().ToConstant(factoryMock.Object);
        }
    }
}
