using Clusterizer.Entities;
using Clusterizer.EntitiesReaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clusterizer.RemoteDatabases
{
    public interface IRemoteDatabase : IEntitiesReader
    {
        bool AddEntity(IEntity entity);
        bool RemoveEntity(IEntity entity);
        IEntity FindEntity(List<List<string>> attributes);

        void DropDatabase();
    }
}
