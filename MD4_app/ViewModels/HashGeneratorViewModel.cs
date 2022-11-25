using MD4_hash;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MD4_app.ViewModels
{
    internal enum HashCompareResult
    {
        None, Equal, NotEqual, WrongLength, WrongSymbol
    }

    internal class HashGeneratorViewModel : BaseViewModel
    {
        public MD4 Hasher = new();

        private string? compareHash;
        private string? compareValue;
        private bool isFileHash;
        private HashCompareResult compareResult = HashCompareResult.None;
        private bool isEnabled = true;
        private bool isPasswordRequired = false;
        private string saltValidationError = "";

        public string Salt
        {
            get => Hasher.Salt;
            set
            {
                Hasher.Salt = value;
                OnPropertyChanged();
                OnPropertyChanged("HexHash");
            }
        }
        public string? Input
        {
            get => Hasher.Value;
            set
            {
                Hasher.Value = value;
                OnPropertyChanged();
                OnPropertyChanged("HexHash");
            }
        }
        public string HexHash
        {
            get => Hasher.HexHash.ToUpper();
            set
            {
                Hasher.Value = Hasher.Value;
                OnPropertyChanged();
            }
        }
        public string? CompareValue
        {
            get => compareValue;
            set
            {
                compareValue = value;
                OnPropertyChanged();
                OnPropertyChanged("CompareResultString");
                OnPropertyChanged("CompareResultStringColor");
                OnPropertyChanged("CompareResult");
                OnPropertyChanged("ComparisionVisibility");
            }
        }
        public string? CompareHashHex
        {
            get => compareHash;
            set
            {
                compareHash = value == null || value == "" ? null : value.ToUpper();
                OnPropertyChanged();
                OnPropertyChanged("CompareResultString");
                OnPropertyChanged("CompareResultStringColor");
                OnPropertyChanged("CompareResult");
                OnPropertyChanged("ComparisionVisibility");
            }
        }
        public HashCompareResult CompareResult
        {
            get => compareResult;
            set
            {
                compareResult = value;
                OnPropertyChanged();
                OnPropertyChanged("CompareResultString");
                OnPropertyChanged("CompareResultStringColor");
                OnPropertyChanged("ComparisionVisibility");
            }
        }
        public bool IsPasswordRequired
        {
            get => isPasswordRequired;
            set
            {
                isPasswordRequired = value;
                if (!isPasswordRequired)
                    Salt = "";
                OnPropertyChanged();
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
                OnPropertyChanged("InputBackground");
                OnPropertyChanged("IsProgressing");
            }
        }
        public bool IsFileHasher
        {
            get => isFileHash;
            set
            {
                isFileHash = value;
                if (isFileHash)
                    SetFileHasher(null);
                else
                    SetStringHasher();
            }
        }
        public string SaltValidationError
        {
            get => saltValidationError;
            set
            {
                saltValidationError = value;
                OnPropertyChanged();
            }
        }

        public string? CompareResultString => compareResult switch
        {
            HashCompareResult.Equal => "хеши совпадают",
            HashCompareResult.NotEqual => "хеши не совпадают",
            HashCompareResult.WrongLength => "недопустимая длина (требуется 32 символов)",
            HashCompareResult.WrongSymbol => "недопустимый символ",
            _ => null
        };
        public Brush CompareResultStringColor => compareResult switch
        {
            HashCompareResult.Equal => Brushes.Green,
            HashCompareResult.NotEqual or HashCompareResult.WrongLength or HashCompareResult.WrongSymbol => Brushes.Red,
            _ => Brushes.Black,
        };
        public bool IsInputFieldEnabled => IsEnabled && !IsFileHasher;
        public Visibility ComparisionVisibility => string.IsNullOrEmpty(CompareHashHex) ? Visibility.Collapsed : Visibility.Visible;

        public void SetFileHasher(string filename)
        {
            Hasher = new FileMD4(filename, Salt);
            OnPropertyChanged("Input");
            OnPropertyChanged("IsFileHasher");
            OnPropertyChanged("InputHorAligment");
            OnPropertyChanged("InputVerAligment");
            OnPropertyChanged("InputVeInputFontStylerAligment");
            OnPropertyChanged("IsInputFieldEnabled");
            OnPropertyChanged("InputBackground");
            OnPropertyChanged("IsStringHasher");
            OnPropertyChanged("HexHash");
            OnPropertyChanged("ComparisionVisibility");
        }

        public void SetStringHasher()
        {
            Hasher = new MD4(null, Salt);
            OnPropertyChanged("Input");
            OnPropertyChanged("IsFileHasher");
            OnPropertyChanged("InputHorAligment");
            OnPropertyChanged("InputVerAligment");
            OnPropertyChanged("InputVeInputFontStylerAligment");
            OnPropertyChanged("IsInputFieldEnabled");
            OnPropertyChanged("InputBackground");
            OnPropertyChanged("IsStringHasher");
            OnPropertyChanged("HexHash");
            OnPropertyChanged("ComparisionVisibility");
        }

        public void CalcHash()
        {
            Hasher.Calculate();
            OnPropertyChanged("HexHash");
            OnPropertyChanged("HashBytesString");
            OnPropertyChanged("ComparisionVisibility");
        }

    }
}
