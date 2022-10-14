using System.Runtime.Serialization;

namespace MD4_hash
{
    /// <summary>Base class for MD4 implementations</summary>
    [Serializable]
    public partial class MD4
    {
        /// <summary>The size in bytes of the input block to the transformation algorithm</summary>
        protected const int BLOCK_LENGTH = 64; // = 512 / 8

        /// <summary>512-bit work buffer = 16 x 32-bit words</summary>
        protected readonly uint[] X = new uint[16];

        /// <summary>4 32-bit words (interim result)</summary>
        protected readonly uint[] context = new uint[4];

        /// <summary> 512-bit input buffer = 16 x 32-bit words holds until it reaches 512 bits</summary>
        protected byte[] buffer = new byte[BLOCK_LENGTH];

        /// <summary>Number of bytes procesed so far mod. 2 power of 64.</summary>
        protected long count;


        protected byte[]? lastHash = null;

        protected string? value = null;

        protected string salt = "";



        /// <summary>
        ///   Resets this object disregarding any temporary data present at the time of the invocation of this call.
        /// </summary>
        protected void EngineReset()
        {
            context[0] = 0x67452301;
            context[1] = 0xEFCDAB89;
            context[2] = 0x98BADCFE;
            context[3] = 0x10325476;
            count = 0L;
            for (int i = 0; i < BLOCK_LENGTH; i++)
                buffer[i] = 0;
        }

        /// <summary>
        ///   MD4 block update operation
        /// </summary>
        /// <remarks>
        ///   Continues an MD4 message digest operation by filling the buffer, transforming data in 512-bit message block(s), updating the variables
        ///   context and count, and leaving the remaining bytes in buffer for the next update or finish.
        /// </remarks>
        protected void EngineUpdate(byte[] input, int offset, int len)
        {
            if (offset < 0 || len < 0 || (long)offset + len > input.Length)
                throw new ArgumentOutOfRangeException();

            var bufferNdx = (int)(count % BLOCK_LENGTH);
            count += len;
            int partLen = BLOCK_LENGTH - bufferNdx;
            int i = 0;
            if (len >= partLen)
            {
                Array.Copy(input, offset + i, buffer, bufferNdx, partLen);

                Transform(ref buffer, 0);

                for (i = partLen; i + BLOCK_LENGTH - 1 < len; i += BLOCK_LENGTH)
                    Transform(ref input, offset + i);
                bufferNdx = 0;
            }

            if (i < len)
                Array.Copy(input, offset + i, buffer, bufferNdx, len - i);
        }

        /// <summary>
        ///   Completes the hash computation by performing final operations. At the return of this engineDigest, the MD engine is reset.
        /// </summary>
        /// <returns>the array of bytes for the resulting hash value.</returns>
        protected byte[] EngineDigest()
        {
            var bufferNdx = (int)(count % BLOCK_LENGTH);
            int padLen = (bufferNdx < 56) ? (56 - bufferNdx) : (120 - bufferNdx);

            var tail = new byte[padLen + 8];
            tail[0] = 0x80;

            for (int i = 0; i < 8; i++)
                tail[padLen + i] = (byte)((count * 8) >> (8 * i));

            EngineUpdate(tail, 0, tail.Length);

            var result = new byte[16];
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    result[i * 4 + j] = (byte)(context[i] >> (8 * j));

            EngineReset();
            return result;
        }

        /// <summary>
        ///   MD4 basic transformation
        /// </summary>
        /// <remarks>
        ///   Transforms context based on 512 bits from input block starting
        ///   from the offset'th byte.
        /// </remarks>
        protected void Transform(ref byte[] block, int offset)
        {
            for (int i = 0; i < 16; i++)
                X[i] = ((uint)block[offset++] & 0xFF) |
                       (((uint)block[offset++] & 0xFF) << 8) |
                       (((uint)block[offset++] & 0xFF) << 16) |
                       (((uint)block[offset++] & 0xFF) << 24);

            uint A = context[0];
            uint B = context[1];
            uint C = context[2];
            uint D = context[3];

            A = FF(A, B, C, D, X[0], 3);
            D = FF(D, A, B, C, X[1], 7);
            C = FF(C, D, A, B, X[2], 11);
            B = FF(B, C, D, A, X[3], 19);
            A = FF(A, B, C, D, X[4], 3);
            D = FF(D, A, B, C, X[5], 7);
            C = FF(C, D, A, B, X[6], 11);
            B = FF(B, C, D, A, X[7], 19);
            A = FF(A, B, C, D, X[8], 3);
            D = FF(D, A, B, C, X[9], 7);
            C = FF(C, D, A, B, X[10], 11);
            B = FF(B, C, D, A, X[11], 19);
            A = FF(A, B, C, D, X[12], 3);
            D = FF(D, A, B, C, X[13], 7);
            C = FF(C, D, A, B, X[14], 11);
            B = FF(B, C, D, A, X[15], 19);

            A = GG(A, B, C, D, X[0], 3);
            D = GG(D, A, B, C, X[4], 5);
            C = GG(C, D, A, B, X[8], 9);
            B = GG(B, C, D, A, X[12], 13);
            A = GG(A, B, C, D, X[1], 3);
            D = GG(D, A, B, C, X[5], 5);
            C = GG(C, D, A, B, X[9], 9);
            B = GG(B, C, D, A, X[13], 13);
            A = GG(A, B, C, D, X[2], 3);
            D = GG(D, A, B, C, X[6], 5);
            C = GG(C, D, A, B, X[10], 9);
            B = GG(B, C, D, A, X[14], 13);
            A = GG(A, B, C, D, X[3], 3);
            D = GG(D, A, B, C, X[7], 5);
            C = GG(C, D, A, B, X[11], 9);
            B = GG(B, C, D, A, X[15], 13);

            A = HH(A, B, C, D, X[0], 3);
            D = HH(D, A, B, C, X[8], 9);
            C = HH(C, D, A, B, X[4], 11);
            B = HH(B, C, D, A, X[12], 15);
            A = HH(A, B, C, D, X[2], 3);
            D = HH(D, A, B, C, X[10], 9);
            C = HH(C, D, A, B, X[6], 11);
            B = HH(B, C, D, A, X[14], 15);
            A = HH(A, B, C, D, X[1], 3);
            D = HH(D, A, B, C, X[9], 9);
            C = HH(C, D, A, B, X[5], 11);
            B = HH(B, C, D, A, X[13], 15);
            A = HH(A, B, C, D, X[3], 3);
            D = HH(D, A, B, C, X[11], 9);
            C = HH(C, D, A, B, X[7], 11);
            B = HH(B, C, D, A, X[15], 15);

            context[0] += A;
            context[1] += B;
            context[2] += C;
            context[3] += D;
        }


        protected static uint FF(uint a, uint b, uint c, uint d, uint x, int s)
        {
            uint t = a + ((b & c) | (~b & d)) + x;
            return t << s | t >> (32 - s);
        }

        protected static uint GG(uint a, uint b, uint c, uint d, uint x, int s)
        {
            uint t = a + ((b & (c | d)) | (c & d)) + x + 0x5A827999;
            return t << s | t >> (32 - s);
        }

        protected static uint HH(uint a, uint b, uint c, uint d, uint x, int s)
        {
            uint t = a + (b ^ c ^ d) + x + 0x6ED9EBA1;
            return t << s | t >> (32 - s);
        }
    }
}
