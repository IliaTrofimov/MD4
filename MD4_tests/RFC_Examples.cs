using NUnit.Framework;
using MD4_hash;

namespace MD4_tests
{
    public class RFC_Examples
    {
        /// <summary>
        /// �������� ������ ��������� MD4 �� �������� �� RFC
        /// </summary>
        /// <remarks>
        /// ���������� ������ '' (������ ������)
        /// </remarks>
        [Test]
        public void ExapmlesRFC_emptystr_test()
        {
            TestString("", "31d6cfe0d16ae931b73c59d7e0c089c0");
        }

        /// <summary>
        /// �������� ������ ��������� MD4 �� �������� �� RFC
        /// </summary>
        /// <remarks>
        /// ���������� ������ 'a'
        /// </remarks>
        [Test]
        public void ExapmlesRFC_a_test()
        {
            TestString("a", "bde52cb31de33e46245e05fbdbd6fb24");
        }

        /// <summary>
        /// �������� ������ ��������� MD4 �� �������� �� RFC
        /// </summary>
        /// <remarks>
        /// ���������� ������ 'abc'
        /// </remarks>
        [Test]
        public void ExapmlesRFC_abc_test()
        {
            TestString("abc", "a448017aaf21d8525fc10ae87aa6729d");
        }

        /// <summary>
        /// �������� ������ ��������� MD4 �� �������� �� RFC
        /// </summary>
        /// <remarks>
        /// ���������� ������ 'message digest'
        /// </remarks>
        [Test]
        public void ExapmlesRFC_msgdigest_test()
        {
            TestString("message digest", "d9130a8164549fe818874806e1c7014b");
        }

        /// <summary>
        /// �������� ������ ��������� MD4 �� �������� �� RFC
        /// </summary>
        /// <remarks>
        /// ���������� ������ 'abcdefghijklmnopqrstuvwxyz'
        /// </remarks>
        [Test]
        public void ExapmlesRFC_alphabet_test()
        {
            TestString("abcdefghijklmnopqrstuvwxyz", "d79e1c308aa5bbcdeea8ed63df412da9");
        }

        /// <summary>
        /// �������� ������ ��������� MD4 �� �������� �� RFC
        /// </summary>
        /// <remarks>
        /// ���������� ������ 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789'
        /// </remarks>
        [Test]
        public void ExapmlesRFC_alphabet_and_digits_test()
        {
            TestString("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789", "043f8582f241db351ce627e153e7f0e4");
        }

        /// <summary>
        /// �������� ������ ��������� MD4 �� �������� �� RFC
        /// </summary>
        /// <remarks>
        /// ���������� ������ '12345678901234567890123456789012345678901234567890123456789012345678901234567890'
        /// </remarks>
        [Test]
        public void ExapmlesRFC_many_digits_test()
        {
            TestString("12345678901234567890123456789012345678901234567890123456789012345678901234567890", "e33b4ddc9c38f2199c3e7b164fcc0536");
        }


        private void TestString(string text, string expectedHash)
        {
            MD4 hasher = new MD4();
            Assert.AreEqual(expectedHash, hasher.GetHexHash(text, false), $"�������� ��� ��� ������:\n'{text}'");
        }
    }
}