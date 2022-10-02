using MD4_app.ViewModels;
using MD4_hash;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MD4_app.Utility
{
    public static class HistorySerializer
    {
        public static string Serizalize(HashHistoryItemViewModel hash)
        {
            return $"{Encoding.UTF8.GetString(hash.BytesHash)}\t{hash.IsFile}\t{hash.Value}";
        }

        public static HashHistoryItemViewModel? Deserialize(string str) 
        {
            string[] tokens = str.Split('\t', 3);
            try
            {
                return new (tokens[2], Encoding.UTF8.GetBytes(tokens[0]), Convert.ToBoolean(tokens[1]));  
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static void SerializeFile(HashHistoryItemViewModel hash, string filename, bool append = false)
        {
            using StreamWriter sw = new StreamWriter(filename, append);
            sw.WriteLine(Serizalize(hash));
        }

        public static void SerializeFile(ICollection<HashHistoryItemViewModel> hashes, string filename, bool append = false)
        {
            using StreamWriter sw = new StreamWriter(filename, append);
            foreach(var hash in hashes)
                sw.WriteLine(Serizalize(hash));
        }

        public static List<HashHistoryItemViewModel> DeserializeFile(string filename)
        {
            using StreamReader sr = new StreamReader(filename);
            List<HashHistoryItemViewModel> hashes = new();
            for (string? line = sr.ReadLine(); line != null; line = sr.ReadLine())
            {
                var hash = Deserialize(line);
                if (hash != null)
                    hashes.Add(hash);
            }
            return hashes;
        }
    }
}
