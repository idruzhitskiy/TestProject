using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Clusterizer.Entities;

namespace Clusterizer.EntitiesReader
{
    public class Reader : IEntitiesReader
    {
        string path;
        public List<IEntity> Entities { get; }

        Reader(string file_path)
        {
            if (file_path == null)
                throw new System.ArgumentNullException();
            path = file_path;
        }

        private List<List<string>> parse()
        {
            List<List<string>> full_strs = new List<List<string>>(); 
            List<string> strs = new List<string>();
            string line;
                
            System.IO.StreamReader file = new System.IO.StreamReader(path);
            while ((line = file.ReadLine()) != null)           
                strs.Add(line);

            /**/
            string text = System.IO.File.ReadAllText(path);
            //if (text == null || text == "")
            //  throw new EmptyFileException();

            string pat1 = @"^#\s*\w+\s*$";
            string pat2 = @"^-\s*";
            Regex r_names = new Regex(pat1, RegexOptions.IgnoreCase);
            Regex r_atrs = new Regex(pat2, RegexOptions.IgnoreCase);
            MatchCollection matches = r_names.Matches(text);

            Console.WriteLine("Found {0} objects", matches.Count);

            for (int i = 0; i < matches.Count-1; i++)
            {
                int count = matches[i + 1].Index - matches[i].Index - matches[i].Length - 1;
                MatchCollection atrs_mathces = r_atrs.Matches()
            }


            char[] delimiterChars = { ' ', ',', '.', ':', '\t' };

            foreach (string str in strs)
            {
                string[] words = str.Split(delimiterChars);
                List<string> _words = new List<string>();
                foreach (string word in words)               
                    _words.Add(word);
                full_strs.Add(_words);
            }
            return full_strs; 
        } 


    }
}
