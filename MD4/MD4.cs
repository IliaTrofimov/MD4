using System.Runtime.Serialization;
using System.Text;

namespace MD4_hash
{
    /// <summary>Base class for MD4 implementations</summary>
    [Serializable]
    public class MD4 : MD4Base, IHasher
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


        public MD4(string? s = null, string salt = "")
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

        public byte[] GetByteHash(string s)
        {
            value = s;
            ProgressChangedHandler?.Invoke(new HashingProgress(HashingStatus.Initializing));
            byte[] b = Encoding.UTF8.GetBytes(salt + s);

            EngineReset();
            EngineUpdate(b, 0, b.Length);
            lastHash = EngineDigest();
            ProgressChangedHandler?.Invoke(new HashingProgress(HashingStatus.Done));
            return lastHash;
        }

        public Task<byte[]?> GetByteHashAsync(string s, CancellationToken cancellationToken)
        {
            value = s;
            ProgressChangedHandler?.Invoke(new HashingProgress(HashingStatus.Initializing));
            return Task.Run(() =>
            {
                try
                {
                    byte[] b = Encoding.UTF8.GetBytes(salt + s);

                    EngineReset();
                    EngineUpdate(b, 0, b.Length);
                    lastHash = EngineDigest();
                    ProgressChangedHandler?.Invoke(new HashingProgress(HashingStatus.Done));
                    return lastHash;
                }
                catch (OperationCanceledException)
                {
                    ProgressChangedHandler?.Invoke(new HashingProgress(HashingStatus.Cancelled));
                    EngineReset();
                    return lastHash = null;
                }
            }, cancellationToken); 
        }

        public string GetHexHash(string s, bool upperCase = true)
        {
            GetByteHash(s);
            return MD4Utility.ToHex(lastHash, upperCase);
        }



        protected override void SetProgress(long processed, long total)
        {
            ProgressChangedHandler?.Invoke(new HashingProgress(processed, total, HashingStatus.Processing));
        }
    }
}

