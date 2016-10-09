using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Clusterizer.Entities;
using System.Text;

namespace Clusterizer.RemoteDatabases
{
    public class FileRemoteDatabase : IRemoteDatabase
    {
        private readonly IEntitiesFactory entitiesFactory;

        public FileRemoteDatabase(IEntitiesFactory entitiesFactory)
        {
            this.entitiesFactory = entitiesFactory;
        }

        public bool AddEntity(IEntity entity)
        {
            using (var f = new StreamWriter("db.txt"))
            {
                var str = entity.Id + "=" + string.Join(";", entity.TextAttributes.Select(l => string.Join(" ", l)));
                f.WriteLine(str);
                return true;
            }
        }

        public IEntity FindEntity(List<List<string>> attributes)
        {
            using (var f = new StreamReader("db.txt"))
            {
                var str = string.Join(";", attributes.Select(l => string.Join(" ", l)));
                string curStr = null;
                while (!string.IsNullOrWhiteSpace(curStr = f.ReadLine()))
                {
                    if (curStr.Contains(str))
                    {
                        return entitiesFactory.CreateEntityWithId(
                            curStr.Split(new char[] { '=' })[1]
                                .Split(new char[] { ';' })
                                .Select(s => s.Split(new char[] { ' ' }).ToList())
                                .ToList(), 
                            curStr.Split(new char[] { '=' })[0]);
                    }
                }
                return null;
            }
        }

        public bool RemoveEntity(IEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
