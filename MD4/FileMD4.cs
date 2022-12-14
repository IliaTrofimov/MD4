using System.Text;

namespace MD4_hash
{
    /// <summary>Implements the MD4 message digest algorithm for hashing files</summary>
    public class FileMD4 : MD4Base, IHasher
    {
        protected byte[]? lastHash = null;

        protected string? value = null;

        protected string salt = "";


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
        public Action<HashingProgress>? ProgressChangedHandler { get; set; }


        public FileMD4(string? s = null, string salt = "")
        {
            EngineReset();
            value = s;
            this.salt = salt;
        }



        public void Calculate()
        {
            GetByteHash(value is null ? "" : value);
        }

        public Task CalculateAsync(CancellationToken cancellationToken)
        {
            return GetByteHashAsync(value is null ? "" : value, cancellationToken);
        }

        public byte[] GetByteHash(string filename)
        {
            value = filename;
            ProgressChangedHandler?.Invoke(new HashingProgress(HashingStatus.Initializing));

            using FileStream fs = new FileStream(filename, FileMode.Open);
            byte[] b = new byte[salt.Length + fs.Length];
            if (!string.IsNullOrEmpty(salt)) 
            {
                byte[] s = Encoding.UTF8.GetBytes(salt);
                Array.Copy(s, b, s.Length);
            }
            fs.Read(b, salt.Length, (int)fs.Length);

            EngineReset();
            EngineUpdate(b, 0, b.Length);
            lastHash = EngineDigest();
            ProgressChangedHandler?.Invoke(new HashingProgress(HashingStatus.Done));
            return lastHash;
        }

        public async Task<byte[]?> GetByteHashAsync(string filename, CancellationToken cancellationToken)
        {
            value = filename;
            ProgressChangedHandler?.Invoke(new HashingProgress(HashingStatus.Initializing));

            try
            {
                byte[] file = await File.ReadAllBytesAsync(value, cancellationToken);
                byte[] b = new byte[file.Length + salt.Length];
                if (!string.IsNullOrEmpty(salt))
                {
                    byte[] s = Encoding.UTF8.GetBytes(salt);
                    Array.Copy(s, b, s.Length);
                }
                Array.Copy(file, b, file.Length);

                EngineReset();
                EngineUpdate(b, 0, b.Length);
                lastHash = EngineDigest();
                return lastHash = EngineDigest();
            }
            catch (OperationCanceledException)
            {
                ProgressChangedHandler?.Invoke(new HashingProgress(HashingStatus.Cancelled));
                EngineReset();
                return lastHash = null;
            }
            
        }
        
        public string GetHexHash(string filename, bool upperCase = true)
        {
            GetByteHash(filename);
            return MD4Utility.ToHex(lastHash, upperCase);
        }


        protected override void SetProgress(long processed, long total)
        {
            ProgressChangedHandler?.Invoke(new HashingProgress(processed, total, HashingStatus.Processing));
        }
    }
}

