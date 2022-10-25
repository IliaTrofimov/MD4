using System;
using System.IO;
using System.Text;

namespace MD4_app.Utility
{
    internal static class FileIO
    {
        public static string ReadHashFile(string filename)
        {
            var data = File.ReadAllBytes(filename);
            if (data == null)
                throw new FileFormatException("Не удалось прочитать файл с контрольной суммой");
            else if (data.Length != 32)
                throw new FileFormatException($"Файл с контрольной суммой должен содержать ровно 32 байт данных (прочитано {data.Length} байт) ");
            return Encoding.UTF8.GetString(data);
        }

        public static void WriteHashFile(MD4_hash.MD4 hasher, string filename)
        {
            File.WriteAllText(filename, hasher.HexHash);
        }

        public static string GenerateHashFileName(string value, bool isFileHash = false)
        {
            return !isFileHash ? $"hash_str_{value[0..Math.Min(value.Length, 30)]}_.txt" : $"hash_file_{Path.GetFileNameWithoutExtension(value)}_.txt";
        }
    }
}
