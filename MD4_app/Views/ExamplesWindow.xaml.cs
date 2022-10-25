using MD4_app.ViewModels;
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
using System.Windows.Shapes;

namespace MD4_app.Views
{
    /// <summary>
    /// Логика взаимодействия для ExamplesWindow.xaml
    /// </summary>
    public partial class ExamplesWindow : Window
    {
        public ObservableCollection<HashExampleViewModel> Examples { get; private set; }
        public HashExampleViewModel SelectedItem { get; private set; } = new(null, null);
        public ExamplesWindow()
        {
            InitializeComponent();
            Examples = new ObservableCollection<HashExampleViewModel>()
            {
                new(@"123456789012345678901234567890123456789012345678901234567890123456
78901234567890", "eb0a6d76f6c4ce0eb89a6c2133dc3409"),
                new("", "31d6cfe0d16ae931b73c59d7e0c089c0"),
                new("a", "bde52cb31de33e46245e05fbdbd6fb24"),
                new("abc", "a448017aaf21d8525fc10ae87aa6729d"),
                new("message digest", "d9130a8164549fe818874806e1c7014b"),
                new("abcdefghijklmnopqrstuvwxyz", "d79e1c308aa5bbcdeea8ed63df412da9"),
                new("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789", "043f8582f241db351ce627e153e7f0e4"),
                new("12345678901234567890123456789012345678901234567890123456789012345678901234567890", "e33b4ddc9c38f2199c3e7b164fcc0536"),
            };
            listBox_examples.ItemsSource = Examples;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (listBox_examples.SelectedItem != null)
            {
                SelectedItem = (HashExampleViewModel)listBox_examples.SelectedItem;
                DialogResult = true;
                Close();
            }
            else
            {
                System.Media.SystemSounds.Exclamation.Play();
            }
        }
    }
}
