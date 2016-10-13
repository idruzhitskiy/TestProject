using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Clusterizer.Entities;
using System.Text;
using Clusterizer.EntitiesReaders;

namespace Clusterizer.RemoteDatabases
{
    public class FileRemoteDatabase : IRemoteDatabase
    {
        private const string dbFile = "db.txt";
        private readonly IEntitiesFactory entitiesFactory;

        public FileRemoteDatabase(IEntitiesFactory entitiesFactory)
        {
            this.entitiesFactory = entitiesFactory;
            try
            {
                new StreamReader(dbFile).Close();
            }
            catch
            {
                new StreamWriter(dbFile, false).Close();
            }
        }

        public List<IEntity> Entities
        {
            get
            {
                try
                {
                    using (var f = new StreamReader(dbFile))
                    {
                        var result = new List<IEntity>();
                        string curStr = null;
                        while (!string.IsNullOrWhiteSpace(curStr = f.ReadLine()))
                        {
                            if (curStr.Split(new char[] { '=' }).Count() == 2)
                            {
                                result.Add(entitiesFactory.CreateEntityWithId(
                                    curStr.Split(new char[] { '=' })[1]
                                        .Split(new char[] { ';' })
                                        .Select(s => s.Split(new char[] { ' ' }).ToList())
                                        .ToList(),
                                    curStr.Split(new char[] { '=' })[0]));
                            }
                        }
                        return result;
                    }
                }
                catch
                {
                    return new List<IEntity>();
                }
            }
        }

        public bool AddEntity(IEntity entity)
        {
            try
            {
                using (var f = new StreamWriter(dbFile, true))
                {
                    var str = entity.Id + "=" + string.Join(";", entity.TextAttributes.Select(l => string.Join(" ", l)));
                    f.WriteLine(str);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public void DropDatabase()
        {
            new StreamWriter(dbFile, false).Close();
        }

        public IEntity FindEntity(List<List<string>> attributes)
        {
            try
            {
                using (var f = new StreamReader(dbFile))
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
            catch
            {
                return null;
            }
        }

        public bool RemoveEntity(IEntity entity)
        {
            try
            {
                var result = false;
                using (var memoryStream = new MemoryStream())
                {
                    using (var outStream = new StreamWriter(memoryStream))
                    {
                        using (var f = new StreamReader(dbFile))
                        {
                            var id = entity.Id;
                            string curStr = null;
                            while (!string.IsNullOrWhiteSpace(curStr = f.ReadLine()))
                            {
                                if (!curStr.Contains(id))
                                {
                                    outStream.WriteLine(curStr);
                                }
                                else
                                {
                                    result = true;
                                }
                            }
                        }
                        outStream.Flush();
                        using (var outFile = new StreamWriter(dbFile))
                        {
                            using (var outReader = new StreamReader(new MemoryStream(memoryStream.ToArray())))
                            {
                                string curStr = null;
                                while (!string.IsNullOrWhiteSpace(curStr = outReader.ReadLine()))
                                    outFile.WriteLine(curStr);
                            }
                        }
                    }

                }
                return result;
            }
            catch
            {
                return false;
            }
        }
    }
}
