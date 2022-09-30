using System.Text;

namespace MD4_hash
{
    /// <summary>Base class for MD4 implementations</summary>
    public partial class MD4
    {
        public byte[]? BytesHash => lastHash;
        public string HexHash => MD4Utility.ToHex(lastHash);
        public string? Value
        {
            get => value;
            set
            {
                this.value = value;
                lastHash = null;
            }
        }
        public string Salt
        {
            get => salt;
            set
            {
                salt = value;
                lastHash = null;
            }
        }


        public MD4(string? s = null, string salt = "")
        {
            EngineReset();
            value = s;
            this.salt = salt;
        }


        /// <summary>Recalculates hash</summary>
        public virtual void Calculate()
        {
            GetByteHash(value is null ? "" : value);
        }


        /// <summary>Returns a byte hash from a string</summary>
        public virtual byte[] GetByteHash(string s)
        {
            value = s;
            byte[] b = Encoding.UTF8.GetBytes(salt + s);
           
            EngineReset();
            EngineUpdate(b, 0, b.Length);
            return lastHash = EngineDigest();
        }

        /// <summary>Returns a string that contains the hexadecimal hash</summary>
        public virtual string GetHexHash(string s, bool upperCase = false)
        {
            GetByteHash(s);
            return MD4Utility.ToHex(lastHash, upperCase);
        }


        public static bool operator==(MD4 h1, MD4 h2)
        {
            return h1 == h2.lastHash;
        }

        public static bool operator !=(MD4 h1, MD4 h2)
        {
            return !(h1 == h2.lastHash);
        }

        public static bool operator ==(MD4 h1, byte[] h2)
        {
            if (h1.lastHash == null && h2 == null)
                return true;
            else if (h1.lastHash != null && h2 != null)
            {
                for (int i = 0; i < h1.lastHash.Length && i < h2.Length; i++)
                    if (h1.lastHash[i] != h2[i]) return false;
                return true;
            }

            return false;
        }

        public static bool operator !=(MD4 h1, byte[] h2)
        {
            return !(h1 == h2);
        } 
    }
}
