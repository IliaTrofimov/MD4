using System;
using System.IO;


namespace MD4_app.Utility
{
    internal static class FileIO
    {
        public static string ReadHashFile(string filename)
        {
            return File.ReadAllText(filename);
        }

        public static void WriteHashFile(string hexhash, string filename)
        {
            File.WriteAllText(filename, hexhash);
        }

        public static string GenerateHashFileName(string value, bool isFileHash = false)
        {
            return !isFileHash ? $"hash_str_{value[0..Math.Min(value.Length, 30)]}_.txt" : $"hash_file_{Path.GetFileNameWithoutExtension(value)}_.txt";
        }
    }
}
