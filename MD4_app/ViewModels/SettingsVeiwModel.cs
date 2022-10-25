using MD4_app.Utility.PasswordValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD4_app.ViewModels
{
    internal class SettingsVeiwModel : BaseViewModel
    {
        public PasswordSymbolsRestriction passwordSymbolsRestriction = new();
        private bool isPasswordRequired = false;

        public SettingsVeiwModel(PasswordSymbolsRestriction restriction, bool isRequired)
        {
            this.passwordSymbolsRestriction = restriction;
            this.isPasswordRequired = isRequired; 
        }

        public int PasswordMinLength
        {
            get => passwordSymbolsRestriction.MinLength;
            set
            {
                passwordSymbolsRestriction.MinLength = value;
                OnPropertyChanged();
            }
        }
        public bool MustHaveCyryllicSymbols
        {
            get => passwordSymbolsRestriction.MustHaveCyryllicSymbols;
            set
            {
                passwordSymbolsRestriction.MustHaveCyryllicSymbols = value;
                OnPropertyChanged();
            }
        }
        public bool MustHaveLatinSymbols
        {
            get => passwordSymbolsRestriction.MustHaveLatinSymbols;
            set
            {
                passwordSymbolsRestriction.MustHaveLatinSymbols = value;
                OnPropertyChanged();
            }
        }
        public bool MustHaveDigits
        {
            get => passwordSymbolsRestriction.MustHaveDigits;
            set
            {
                passwordSymbolsRestriction.MustHaveDigits = value;
                OnPropertyChanged();
            }
        }
        public bool MustHaveUpperCase
        {
            get => passwordSymbolsRestriction.MustHaveUpperCase;
            set
            {
                passwordSymbolsRestriction.MustHaveUpperCase = value;
                OnPropertyChanged();
            }
        }
        public bool MustHaveSpecialSymbols
        {
            get => passwordSymbolsRestriction.MustHaveSpecialSymbols;
            set
            {
                passwordSymbolsRestriction.MustHaveSpecialSymbols = value;
                OnPropertyChanged();
            }
        }
        public bool IsPasswordRequired
        {
            get => isPasswordRequired;
            set
            {
                isPasswordRequired = value;
                OnPropertyChanged();
            }
        }
    }
}
