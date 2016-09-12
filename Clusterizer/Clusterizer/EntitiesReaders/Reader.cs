using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clusterizer.Entities;
using System.Text.RegularExpressions;

namespace Clusterizer.EntitiesReaders
{
    public class Reader : IEntitiesReader
    {
        public List<IEntity> Entities { get; }

        public Reader(IEntitiesFactory factory, System.IO.TextReader reader)
        {
            this.parse(reader.ReadToEnd(), factory);
        }

        private int parse(string text, IEntitiesFactory factory)
        {
            string pat1 = @"^#\s*\w+\s*$";  // name of object
            string pat2 = @"^-\s*.$";       // strings with atribs
            Regex r_names = new Regex(pat1, RegexOptions.IgnoreCase);
            Regex r_atrs = new Regex(pat2, RegexOptions.IgnoreCase);
            MatchCollection name_matches = r_names.Matches(text);
            MatchCollection atrs_mathces;
            int start, count;

            Console.WriteLine("Found {0} objects", name_matches.Count);
            //string name;

            for (int i = 0; i < name_matches.Count - 1; i++)
            {
                start = name_matches[i].Index;
                count = name_matches[i + 1].Index - start;
                //name = name_matches[i].Value.Substring(1);

                // находим все строки с атрибутами 
                atrs_mathces = r_atrs.Matches(text.Substring(start, count));
                
                // создаем сущность с атрибутами
                this.Entities.Add(factory.CreateEntity(this.get_words(atrs_mathces)));
                //this.Entities.Add(factory.CreateEntity(name, this.get_words(atrs_mathces)));                
            }
            
            start = name_matches[name_matches.Count - 1].Index;
            //name = name_matches[name_matches.Count - 1].Value.Substring(1);

            // находим все строки с атрибутами для последнего объекта
            atrs_mathces = r_atrs.Matches(text.Substring(start, text.Length - start));

            // создаем сущность с атрибутами
            this.Entities.Add(factory.CreateEntity(this.get_words(atrs_mathces)));
            //this.Entities.Add(factory.CreateEntity(name, this.get_words(atrs_mathces)));

            return 1;
        }

        private List<List<string>> get_words(MatchCollection matches)
        {
            List<List<string>> atrs = new List<List<string>>();
            char[] delimiterChars = { ' ', ',', '.', ':', '\t' };

            for (int j = 0; j < matches.Count; j++)
            {
                string[] _words = matches[j].Value.Split(delimiterChars);
                List<string> words = new List<string>();
                foreach (string word in _words)
                    words.Add(word);
                atrs.Add(words);
            }

            return atrs;
        }

    }
}
