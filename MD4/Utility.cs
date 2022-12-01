using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD4_hash
{
    public enum HashingStatus
    {
        Done, Initializing, Processing, Cancelled
    }

    public class HashingProgress
    {
        public long Processed { get; set; }
        public long Total { get; set; }
        public HashingStatus Status { get; set; }
        public double Progress => Status == HashingStatus.Processing
            ? (double)Processed / Total * 100
            : 0;


        public HashingProgress(HashingStatus status)
        {
            Status = status;
            Processed = 0;
            Total = -1;
        }

        public HashingProgress(long processed, long total, HashingStatus status)
        {
            Status = status;
            Processed = processed;
            Total = total;
        }


        public override string ToString() => Status switch
        {
            HashingStatus.Initializing => "Инициализация...",
            HashingStatus.Done => "Выполнено",
            HashingStatus.Cancelled => "Отменено",
            HashingStatus.Processing when Total <= 1024 * 5 => $"Обработано {Processed} из {Total} байт",
            HashingStatus.Processing when Total <= 1024 * 1024 * 5 => $"Обработано {Processed / 1024} из {Total / 1024} КБ",
            HashingStatus.Processing => $"Обработано {Processed / 1024 / 1024} из {Total / 1024 / 1024} МБ",
            _ => ""
        };
    }

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
