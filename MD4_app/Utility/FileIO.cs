using System;
using System.IO;
using System.Text;

namespace MD4_app.Utility
{
    internal static class FileIO
    {
        public static string ReadHashFile(string filename)
        {
            var data = File.ReadAllText(filename, Encoding.UTF8);
            if (data == null)
                throw new FileFormatException("Не удалось прочитать файл с контрольной суммой");
            try
            {
                CheckHash(data);
            }
            catch (FileFormatException ex)
            {
                throw ex;
            }
            return data.ToUpper();
        }

        public static string ReadHashFileUnsafe(string filename)
        {
            var data = File.ReadAllText(filename, Encoding.UTF8);
            if (data == null)
                throw new FileFormatException("Не удалось прочитать файл с контрольной суммой");  
            return data.ToUpper();
        }

        public static void WriteHashFile(MD4_hash.IHasher hasher, string filename)
        {
            File.WriteAllText(filename, hasher.HexHash);
        }

        public static string GenerateHashFileName(string value, bool isFileHash = false)
        {
            return !isFileHash ? $"hash_str_{value[0..Math.Min(value.Length, 30)]}_.txt" : $"hash_file_{Path.GetFileNameWithoutExtension(value)}_.txt";
        }

        public static bool CheckHash(string hash)
        {
            if (hash == null || hash.Length != 32)
                throw new FileFormatException("Контрольная сумма должна содержать ровно 32 символа");
            foreach(char c in hash)
                if (!char.IsDigit(c) && (char.ToLower(c) < 'a' || char.ToLower(c) > 'f'))
                    throw new FileFormatException("Контрольная сумма должна быть 16-ричным числом");
            return true;
        }
    }
}
