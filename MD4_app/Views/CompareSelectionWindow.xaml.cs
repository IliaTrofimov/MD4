using MD4_app.Utility;
using MD4_app.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для CompareSelectionWindow.xaml
    /// </summary>
    public partial class CompareSelectionWindow : Window
    {
        public CompareSelectionWindowViewModel ViewModel { get; set; }
        public string? ComparingFile => ViewModel.ComparingFile;
        public string? ComparingHashValue => ViewModel.ComparingHashValue;

        public CompareSelectionWindow()
        {
            InitializeComponent();
            ViewModel = new();
            DataContext = ViewModel;
        }


        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.ComparingHashValue == null)
                DialogResult = false;
            else
            {
                try
                {
                    FileIO.CheckHash(ViewModel.ComparingHashValue);
                }
                catch (FileFormatException ex)
                {
                    ViewModel.ErrorString = ex.Message;
                    System.Media.SystemSounds.Asterisk.Play();
                    return;
                }
                DialogResult = true;
            }
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void ComparingHashValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.ErrorString = null;
            ViewModel.ComparingFile = null;
        }


        private void fileInput_FileSelected(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.ComparingHashValue = FileIO.ReadHashFile(fileInput.Text);
                ViewModel.ComparingFile = fileInput.Text;
            }
            catch (Exception ex)
            {
                ViewModel.ComparingFile = null;
                ViewModel.ComparingHashValue = null;
                System.Media.SystemSounds.Asterisk.Play();
                ViewModel.ErrorString = ex.Message;
            }
        }
    }
}
