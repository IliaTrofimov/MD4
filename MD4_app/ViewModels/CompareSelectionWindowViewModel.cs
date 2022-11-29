namespace MD4_app.ViewModels
{
    public class CompareSelectionWindowViewModel : BaseViewModel
    {
        private string? comparingFile;
        private string? comparingHashValue;
        private string? errorString;


        public string? ComparingFile { get => comparingFile; set { comparingFile = value; OnPropertyChanged(); } }
        public string? ComparingHashValue { get => comparingHashValue; set { comparingHashValue = value; OnPropertyChanged(); } }
        public string? ErrorString { get => errorString; set { errorString = value; OnPropertyChanged(); } }
    }
}
