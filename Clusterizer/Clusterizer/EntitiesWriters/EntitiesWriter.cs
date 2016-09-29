using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Clusterizer.Entities;

namespace Clusterizer.EntitiesWriters
{
    class EntitiesWriter : IEntitiesWriter
    {
        private readonly TextWriter textWriter;
        private readonly List<List<IEntity>> entities;

        public EntitiesWriter(TextWriter textWriter, List<List<IEntity>> entities)
        {
            this.textWriter = textWriter;
            this.entities = entities;
        }

        public void Write()
        {
            int i = 1;
            foreach (List<IEntity> cluster in entities)
            {
                textWriter.WriteLine("@Кластер {0}", i++);
                int j = 1;
                foreach (IEntity entity in cluster)
                {
                    textWriter.WriteLine("\t#сущность {0}", j++);
                    foreach (List<string> atr in entity.TextAttributes)
                    {                        
                        textWriter.Write("\t-");
                        foreach (string word in atr)
                            textWriter.Write(word + " ");
                        textWriter.WriteLine();
                    }
                }
            }
            textWriter.Flush();
            textWriter.Close();
        }
    }
}
