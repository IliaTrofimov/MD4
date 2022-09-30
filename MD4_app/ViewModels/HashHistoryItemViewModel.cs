using MD4_hash;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MD4_app.ViewModels
{
    public class HashHistoryItemViewModel : BaseViewModel
    {
        private bool isComparing = false;


        public MD4 Hash;
        public bool IsComparing
        {
            get => isComparing;
            set
            {
                isComparing = value;
                OnPropertyChanged();
                OnPropertyChanged("FontWeight");
                OnPropertyChanged("FontColor");
            }
        }


        public HashHistoryItemViewModel(MD4 hash)
        {
            Hash = hash;
        }


        public string HashHex => Hash.HexHash;
        public string? Value => Hash.Value;
        public string Label => Hash is FileMD4 ? "файл" : "строка";
        public Color FontColor => isComparing ? Colors.CadetBlue : Colors.Black;
        public FontWeight FontWeight => isComparing ? FontWeights.Bold : FontWeights.Normal;
    }
}
