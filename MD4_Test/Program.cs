using MD4_hash;
using System.Text;

namespace MD4_Test
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            Console.CursorVisible = false;
            string path = @"D:\Загрузки\lektsii.docx";

            FileMD4 fileMD4 = new FileMD4(path);
            Console.WriteLine(fileMD4.GetHexHash(path));

        }

    }
}