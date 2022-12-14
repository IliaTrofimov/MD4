using NUnit.Framework;
using MD4_hash;
using System.Text;
using System;

namespace MD4_tests
{
    public class Tests
    {
        
        /// <summary>
        /// Проверяем использования пароля.
        /// </summary>
        /// <remarks>
        /// Т.к. "соль" добавляется в начало текста, то hash(salted_str) == hash(salt + str)
        /// </remarks>
        [Test]
        public void Salt_test()
        {
            StringBuilder sb = new StringBuilder();
            Random rnd = new();

            for (int i = 0; i < 50; i++)
                sb.Append((char)rnd.Next('0', 'Z'));
            string salt = sb.ToString();
            sb.Clear();

            for (int i = 0; i < 500; i++)
                sb.Append((char)rnd.Next('0', 'Z'));
            string text = sb.ToString();
           
            MD4 hasher = new MD4(text, salt);
            hasher.Calculate();
            string hash_salted = hasher.HexHash;

            hasher = new(salt + text);
            hasher.Calculate();
            string hash_unsalted = hasher.HexHash;

            Assert.AreEqual(hash_unsalted, hash_salted, "'Солёный' хеш неверный");
        }
    }
}