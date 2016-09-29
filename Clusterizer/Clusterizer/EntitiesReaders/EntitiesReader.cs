using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clusterizer.Entities;
using System.IO;

namespace Clusterizer.EntitiesReaders
{
    public class EntitiesReader : IEntitiesReader
    {
        private readonly IEntitiesFactory entitiesFactory;
        private readonly TextReader textReader;

        public EntitiesReader(IEntitiesFactory entitiesFactory, TextReader textReader)
        {
            this.entitiesFactory = entitiesFactory;
            this.textReader = textReader;
        }

        private List<IEntity> entities;
        public List<IEntity> Entities
        {
            get
            {
                if (entities == null)
                {
                    entities = new List<IEntity>();
                    string temp = textReader.ReadLine();
                    var entityStrings = new List<string>();
                    while (temp != null)
                    {
                        while (temp != null && !temp.TrimStart().StartsWith("#"))
                        {
                            temp = textReader.ReadLine();
                        }
                        temp = textReader.ReadLine();
                        while (temp != null && !temp.TrimStart().StartsWith("#"))
                        {
                            entityStrings.Add(temp);
                            temp = textReader.ReadLine();
                        }
                        if (entityStrings.Count > 0)
                        {
                            entities.Add(entitiesFactory.CreateEntity(
                                entityStrings
                                    .Where(s => s.TrimStart().StartsWith("-"))
                                    .Select(s => s.TrimStart(new[] { ' ', '-' }).Split(' ').Where(str => !string.IsNullOrWhiteSpace(str)).ToList())
                                    .ToList()));
                            entityStrings = new List<string>();
                        }
                    }
                }
                return entities;
            }
        }
    }
}
