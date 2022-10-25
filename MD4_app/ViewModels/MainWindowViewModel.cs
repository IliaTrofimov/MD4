using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace MD4_app.ViewModels
{

    internal class MainWindowViewModel : BaseViewModel
    {
        private bool isComparing = false;
        private HashCompareResult compareResult = HashCompareResult.None;
        private bool isPasswordRequired = false;
        private string saltValidationError = "";

        public HashGeneratorViewModel HasherOneVM { get; set; } = new HashGeneratorViewModel();
        public HashGeneratorViewModel HasherTwoVM { get; set; } = new HashGeneratorViewModel();


        public bool IsPasswordRequired
        {
            get => isPasswordRequired;
            set
            {
                isPasswordRequired = value;
                OnPropertyChanged();
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
        public bool IsComparing
        {
            get => isComparing;
            set
            {
                isComparing = value;
                OnPropertyChanged();
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
            _ => Brushes.Red,
        };


    }
}
