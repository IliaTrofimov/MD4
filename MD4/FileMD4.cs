using System.Text;

namespace MD4_hash
{
    /// <summary>Implements the MD4 message digest algorithm for hashing files</summary>
    public class FileMD4 : MD4
    {
        public string? FileName
        {
            get => value;
            set
            {
                this.value = value;
                lastHash = null;
            }
        }


        public FileMD4(string filename = null, string salt = "") : base(filename, salt) { }


        /// <summary>
        ///   Returns last hash value
        /// </summary>
        /// <returns>byte-array that contains the hash or null if nothing was hashed</returns>
        override public byte[]? GetByteHash()
        {
            if (lastHash == null)
                GetByteHash(Value);
            return lastHash;
        }

        /// <summary>
        ///   Returns a byte hash from a file
        /// </summary>
        /// <param name = "s">name of the file to hash</param>
        /// <returns>byte-array that contains the hash</returns>
        override public byte[] GetByteHash(string filename)
        {
            value = filename;

            using FileStream fs = new FileStream(filename, FileMode.Open);
            byte[] b = new byte[salt.Length + fs.Length];
            fs.Read(b, salt.Length, (int)fs.Length);
           
            EngineReset();
            EngineUpdate(b, 0, b.Length);
            return lastHash = EngineDigest();
        }

        /// <summary>
        ///   Returns a byte hash from a file
        /// </summary>
        /// <param name = "s">name of the file to hash</param>
        /// <returns>byte-array that contains the hash</returns>
        public Task<byte[]> GetByteHashAsync(string filename)
        {
            return Task.Run(() =>
            {
                Value = filename;
                byte[] b = File.ReadAllBytes(filename);
                EngineReset();
                EngineUpdate(b, 0, b.Length);
                return lastHash = EngineDigest();
            });
        }

        /// <summary>
        ///   Returns a byte hash from a file
        /// </summary>
        /// <returns>byte-array that contains the hash</returns>
        public Task<byte[]> GetByteHashAsync()
        {
            return Task.Run(() =>
            {
                if (lastHash == null)
                    lastHash = GetByteHash(Value);
                return lastHash;
            });
        }
    }
}
