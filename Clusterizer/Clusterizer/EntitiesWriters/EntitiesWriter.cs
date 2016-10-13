using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Clusterizer.Entities;

namespace Clusterizer.EntitiesWriters
{
    public class EntitiesWriter : IEntitiesWriter
    {
        private readonly StreamWriter textWriter;

        public EntitiesWriter(StreamWriter textWriter)
        {
            this.textWriter = textWriter;            
        }

        public void Write(List<List<IEntity>> clusters)
        {
            int i = 1;
            foreach (List<IEntity> cluster in clusters)
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
