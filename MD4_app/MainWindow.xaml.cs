using MD4_app.ViewModels;
using MD4_hash;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

namespace MD4_app
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public HashGeneratorViewModel ViewModel { get; set; } = new();


        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new HashGeneratorViewModel();
            DataContext = ViewModel;
        }

        private void SetFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new() { CheckFileExists = true };
            if (openFileDialog.ShowDialog() == true)
            {
                ViewModel.IsFileHasher = true;
                ViewModel.Input = openFileDialog.FileName;
            }
        }

        private void Run_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.IsEnabled = false;
            MD4 hasher = null;

            Task.Run(() => {
                hasher = ViewModel.IsFileHasher ? new FileMD4(ViewModel.Input, ViewModel.Salt) : new MD4(ViewModel.Input, ViewModel.Salt);
                hasher.Calculate();
                ViewModel.HexHash = hasher.HexHash;
            
            }).ContinueWith(t => Dispatcher.Invoke(() =>
            {
                ViewModel.IsEnabled = true;
                if (hasher is not null) ViewModel.HistoryItems.Add(new(hasher));
            }));
        }

        private void ToggleHistory_Click(object sender, RoutedEventArgs e)
        {
            if (groupBox_history.Visibility == Visibility.Collapsed)
            {
                menu_history.Header = "Скрыть историю";
                groupBox_history.Visibility = Visibility.Visible;
            }
            else
            {
                menu_history.Header = "История";
                groupBox_history.Visibility = Visibility.Collapsed;
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.HexHash = "";
            ViewModel.Input = null;
            ViewModel.IsFileHasher = false;
        }
    }
}
