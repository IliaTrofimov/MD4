using MD4_app.ViewModels;
using MD4_hash;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MD4_app.Controls
{
    /// <summary>
    /// Логика взаимодействия для HistoryExpander.xaml
    /// </summary>
    public partial class HistoryExpander : UserControl
    {
        public ObservableCollection<HashHistoryItemViewModel> HashesObservable { get; set; } = new();

        public HistoryExpander()
        {
            InitializeComponent();
            HashesObservable.Add(new HashHistoryItemViewModel("SSSS"));
            HashesObservable.Add(new HashHistoryItemViewModel("SSSS"));
            HashesObservable.Add(new HashHistoryItemViewModel("SSSS"));
            HashesObservable.Add(new HashHistoryItemViewModel("SSSS"));

        }

        private void Expand_Click(object sender, RoutedEventArgs e)
        {
            list_history.Visibility = list_history.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}
