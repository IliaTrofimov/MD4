using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD4_hash
{
    public static class MD4Utility
    {      
        public static string ToHex(byte[]? b, bool upperCase = false)
        {
            if (b == null) return "";

            if (!upperCase)
                return BitConverter.ToString(b).Replace("-", "").ToLower();
            else
                return BitConverter.ToString(b).Replace("-", "").ToUpper() ;
        }

        public static bool CompareBytes(byte[]? a, byte[]? b)
        {
            if (a == null && b == null)
                return true;
            else if (a != null && b != null)
            {
                for (int i = 0; i < a.Length && i < b.Length; i++)
                    if (a[i] != b[i]) return false;
                return true;
            }

            return false;
        }
    }
}
