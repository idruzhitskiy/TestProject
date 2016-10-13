using Clusterizer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clusterizer.RemoteDatabases
{
    public interface IRemoteDatabase
    {
        bool AddEntity(IEntity entity);
        bool RemoveEntity(IEntity entity);
        IEntity FindEntity(List<List<string>> attributes);

        List<IEntity> FindAllEntitites();

        void DropDatabase();
    }
}
