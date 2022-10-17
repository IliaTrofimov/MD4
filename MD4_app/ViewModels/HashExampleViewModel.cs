namespace MD4_app.ViewModels
{
    public class HashExampleViewModel : BaseViewModel
    {
        public string Hash { get; set; }
        public string Message { get; set; }

        public HashExampleViewModel(string message, string hash)
        {
            Message = message;
            Hash = hash == null ? null : hash.ToUpper();
        }
    }
}
