using MD4_hash;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MD4_app.ViewModels
{
    public class HashGeneratorViewModel : BaseViewModel
    {
        private string salt = "";
        private string hexHash = "";
        private string? input;
        private string? compareHash;
        private bool isFileHasher = false;
        private bool isEnabled = true;


        public string Salt
        {
            get => salt;
            set
            {
                salt = value;
                OnPropertyChanged();
            }
        }
        public string? Input
        {
            get => input;
            set
            {
                input = value;
                OnPropertyChanged();
            }
        }
        public string? CompareHashHex
        {
            get => compareHash;
            set
            {
                compareHash = value;
                OnPropertyChanged();
            }
        }
        public string HexHash
        {
            get => hexHash;
            set
            {
                hexHash = value;
                OnPropertyChanged();
            }
        }
        public bool IsFileHasher
        {
            get => isFileHasher;
            set
            {
                isFileHasher = value;
                OnPropertyChanged();
                OnPropertyChanged("InputHorAligment");
                OnPropertyChanged("InputVerAligment");
                OnPropertyChanged("InputVeInputFontStylerAligment");
                OnPropertyChanged("IsInputFieldEnabled");
                OnPropertyChanged("IsStringHasher");
            }
        }
        public bool IsEnabled
        {
            get => isEnabled;
            set
            {
                isEnabled = value;
                OnPropertyChanged();
                OnPropertyChanged("IsInputFieldEnabled");
                OnPropertyChanged("IsProgressing");
            }
        }


        public ObservableCollection<HashHistoryItemViewModel> HistoryItems { get; set; } = new();


        public bool IsStringHasher => !IsFileHasher;
        public bool IsInputFieldEnabled => IsEnabled && !IsFileHasher;
        public FontStyle InputFontStyle => IsFileHasher ? FontStyles.Italic : FontStyles.Normal;
        public HorizontalAlignment InputHorAligment => IsFileHasher ? HorizontalAlignment.Center : HorizontalAlignment.Left;
        public VerticalAlignment InputVerAligment => IsFileHasher ? VerticalAlignment.Center : VerticalAlignment.Top;
        public bool IsProgressing => !IsEnabled;
    }
}
