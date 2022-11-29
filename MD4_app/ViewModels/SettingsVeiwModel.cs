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
        private bool isRestrictionEnabled = false;
        private bool mustHaveCyrillic;
        private bool mustHaveLatin;
        private bool mustHaveSpecial;
        private bool mustHaveUpper;
        private bool mustHaveDigits;
        private int minLength;

        public SettingsVeiwModel(PasswordSymbolsRestriction restriction, bool isRequired)
        {
            mustHaveCyrillic = restriction.MustHaveCyryllicSymbols;
            mustHaveLatin = restriction.MustHaveLatinSymbols;
            mustHaveSpecial = restriction.MustHaveSpecialSymbols;
            mustHaveUpper = restriction.MustHaveUpperCase;
            mustHaveDigits = restriction.MustHaveDigits;
            minLength = restriction.MinLength;
            this.isRestrictionEnabled = isRequired; 
        }

        public int PasswordMinLength
        {
            get => minLength;
            set
            {
                minLength = value;
                OnPropertyChanged();
            }
        }
        public bool MustHaveCyryllicSymbols
        {
            get => mustHaveCyrillic;
            set
            {
                mustHaveCyrillic = value;
                OnPropertyChanged();
            }
        }
        public bool MustHaveLatinSymbols
        {
            get => mustHaveLatin;
            set
            {
                mustHaveLatin = value;
                OnPropertyChanged();
            }
        }
        public bool MustHaveDigits
        {
            get => mustHaveDigits;
            set
            {
                mustHaveDigits = value;
                OnPropertyChanged();
            }
        }
        public bool MustHaveUpperCase
        {
            get => mustHaveUpper;
            set
            {
                mustHaveUpper = value;
                OnPropertyChanged();
            }
        }
        public bool MustHaveSpecialSymbols
        {
            get => mustHaveSpecial;
            set
            {
                mustHaveSpecial = value;
                OnPropertyChanged();
            }
        }
        public bool IsRestrictionEnabled
        {
            get => isRestrictionEnabled;
            set
            {
                isRestrictionEnabled = value;
                OnPropertyChanged();
            }
        }
    }
}
