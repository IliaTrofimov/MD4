using System.Collections.ObjectModel;

namespace MD4_app.ViewModels
{
    public class HashHistoryViewModel : BaseViewModel
    {
        public ObservableCollection<HashHistoryItemViewModel> HashesObservable { get; private set; } = new();
    }
}
