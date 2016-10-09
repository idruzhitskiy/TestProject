using Clusterizer.Entities;
using Clusterizer.RemoteDatabases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        [TestMethod]
        public void BaseTest()
        {
            var remoteDatabase = kernel.Get<IRemoteDatabase>();
            var factory = kernel.Get<IEntitiesFactory>();

            remoteDatabase.AddEntity(factory.CreateEntity(new List<List<string>> { new List<string> { "1", "2" } }));
            var entity = remoteDatabase.FindEntity(new List<List<string>> { new List<string> { "1", "2" } });

            Assert.IsNotNull(entity);
        }
    }
}
