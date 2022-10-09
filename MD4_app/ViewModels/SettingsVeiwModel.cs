using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD4_app.ViewModels
{
    public class SettingsVeiwModel : BaseViewModel
    {
        private string? passwordRegex;
        private int passwordMinLength = 0;
        private int passwordMaxLength = 10;
        private string validationErrorString = "";
        private bool isPasswordRequired = false;

        public string? PasswordRegex
        {
            get => passwordRegex;
            set
            {
                passwordRegex = value;
                OnPropertyChanged();
            }
        }
        public int PasswordMinLength
        {
            get => passwordMinLength;
            set
            {
                passwordMinLength = value;
                OnPropertyChanged();
            }
        }
        public int PasswordMaxLength
        {
            get => passwordMaxLength;
            set
            {
                passwordMaxLength = value;
                OnPropertyChanged();
            }
        }
        public string ValidationErrorString
        {
            get => validationErrorString;
            set
            {
                validationErrorString = value;
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
