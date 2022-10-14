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


        public FileMD4(string? filename = null, string salt = "") : base(filename, salt) { }


        public override object Clone()
        {
            FileMD4 clone = new FileMD4(value, salt);
            clone.value = value is null ? null : (string)value.Clone();
            clone.salt = (string)salt.Clone();
            clone.lastHash = lastHash is null ? null : (byte[]?)lastHash.Clone();
            return clone;
        }

        /// <summary>Returns a byte hash from a file</summary>
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

        /// <summary>Returns a byte hash from a file</summary>
        public Task<byte[]> GetByteHashAsync(string filename)
        {
            return Task.Run(() =>
            {
                value = filename;
                byte[] b = File.ReadAllBytes(filename);
                EngineReset();
                EngineUpdate(b, 0, b.Length);
                return lastHash = EngineDigest();
            });
        }

        /// <summary>Returns a hexidecimal hash from a file</summary>
        public Task<string> GetHexHashAsync(string filename)
        {
            return Task.Run(() =>
            {
                value = filename;
                byte[] b = File.ReadAllBytes(filename);
                EngineReset();
                EngineUpdate(b, 0, b.Length);
                lastHash = EngineDigest();
                return MD4Utility.ToHex(lastHash);
            });
        }
    }
}
