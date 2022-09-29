using MD4_hash;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MD4_app.ViewModels
{
    public class HashHistoryItemViewModel : BaseViewModel
    {
        public MD4 Hash;

        public HashHistoryItemViewModel(string value = null)
        {
            Hash = new(value);
        }

        public string HashHex => Hash.GetHexHash();
        public string? Value => Hash.Value;
        public FontStyle FontStyle => Hash is FileMD4 ? FontStyles.Italic : FontStyles.Normal;
    }
}
