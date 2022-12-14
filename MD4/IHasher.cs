using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD4_hash
{
    public interface IHasher
    {
        /// <summary> Bytes array of hash </summary>
        public byte[]? BytesHash { get; }

        /// <summary> Hexidecimal value of hash </summary>
        public string HexHash { get; }

        /// <summary> Last hashed value </summary>
        public string? Value { get; set; }

        /// <summary> Secret text string that will be added to initial value </summary>
        public string Salt { get; set; }

        /// <summary> Callback that will be called when hashing progress changes </summary>
        public Action<HashingProgress>? ProgressChangedHandler { get; set; }


        /// <summary> Recalculates hash </summary>
        public void Calculate();

        /// <summary> Recalculates hash </summary>
        public Task CalculateAsync(CancellationToken cancellationToken);

        /// <summary> Returns a byte hash from a string </summary>
        public byte[] GetByteHash(string s);

        /// <summary> Returns a byte hash from a string </summary>
        public Task<byte[]> GetByteHashAsync(string s, CancellationToken cancellationToken);

        /// <summary> Returns a string that contains the hexadecimal hash </summary>
        public string GetHexHash(string s, bool upperCase = false);


        /// <summary>
        /// Calculates hashes with data set in hasherA and hasherB and compares results.
        /// </summary>
        /// <param name="hasherA">Hahser instance. hasherA.Value must be set before calling this method!</param>
        /// <param name="hasherB">Hahser instance. hasherA.Value must be set before calling this method!</param>
        /// <returns>True if hasherA.BytesHash equals to hasherB.BytesHash, false otherwise</returns>
        public static bool CompareHashes(IHasher hasherA, IHasher hasherB)
        {
            hasherA.Calculate();
            hasherB.Calculate();
            byte[]? hashABytes = hasherA.BytesHash;
            byte[]? hashBBytes = hasherB.BytesHash;
            
            if (hashABytes is null || hashBBytes is null || hashABytes.Length != hashBBytes.Length)
                return false;

            for (int i = 0; i < hashABytes.Length && i < hashBBytes.Length; i++)
                if (hashABytes[i] != hashBBytes[i]) return false;

            return true;
        } 

    }
}