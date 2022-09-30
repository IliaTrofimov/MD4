using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD4_hash
{
    public static class MD4Utility
    {
        public static string ToHex(byte[] b, bool upperCase = false)
        {
            if (!upperCase)
                return BitConverter.ToString(b).Replace("-", "").ToLower();
            else
                return BitConverter.ToString(b).Replace("-", "").ToUpper() ;
        }
    }
}
