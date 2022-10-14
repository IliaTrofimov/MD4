﻿using MD4_hash;
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
    public class HashGeneratorViewModel : BaseViewModel
    {
        public MD4 Hasher = new();

        private string? compareHash;
        private bool? compareResult;
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
            }
        }
        public string? Input
        {
            get => Hasher.Value;
            set
            {
                Hasher.Value = value;
                OnPropertyChanged();
            }
        }
        public string HexHash
        {
            get => Hasher.HexHash;
            set
            {
                Hasher.Value = Hasher.Value;
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
        public bool? CompareResult
        {
            get => compareResult;
            set
            {
                compareResult = value;
                OnPropertyChanged();
                OnPropertyChanged("CompareResultString");
            }
        }
        public bool IsPasswordRequired
        {
            get => isPasswordRequired;
            set
            {
                isPasswordRequired = value;
                if (!isPasswordRequired)
                {
                    Hasher.Salt = "";
                    OnPropertyChanged("Salt");
                }
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
            null => null,
            true => "хеши совпадают",
            false => "хеши не совпадают"
        };
        public bool IsFileHasher => Hasher is FileMD4;
        public bool IsInputFieldEnabled => IsEnabled && !IsFileHasher;
        public FontStyle InputFontStyle => IsFileHasher ? FontStyles.Italic : FontStyles.Normal;
        public HorizontalAlignment InputHorAligment => IsFileHasher ? HorizontalAlignment.Center : HorizontalAlignment.Left;
        public VerticalAlignment InputVerAligment => IsFileHasher ? VerticalAlignment.Center : VerticalAlignment.Top;


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
        }

        public void CalcHash()
        {
            Hasher.Calculate();
            OnPropertyChanged("HexHash");
        }

    }
}
