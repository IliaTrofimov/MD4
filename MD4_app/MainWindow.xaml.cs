using MD4_app.Utility;
using MD4_app.ViewModels;
using MD4_app.Views;
using Microsoft.Win32;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MD4_app
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public HashGeneratorViewModel ViewModel { get; set; }
        private string lastPassword;

        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new HashGeneratorViewModel();
            DataContext = ViewModel;
            ViewModel.IsPasswordRequired = Properties.Settings.Default.IsPasswordRequired;
            lastPassword = ViewModel.Salt;

            Title = $"MD4 [process id: {System.Environment.ProcessId}]";
        }

        private bool CheckPassword(bool isSilent = false)
        {
            PasswordValidationError res = PasswordValidation.Validate(ViewModel.Salt);
            if (res == PasswordValidationError.Ok)
            {
                ViewModel.SaltValidationError = "";
                lastPassword = ViewModel.Salt;
                return true;
            }
            else if (!isSilent)
            {
                switch (res)
                {
                    case PasswordValidationError.TooLagre:
                        ViewModel.SaltValidationError = $"Пароль должен содержать не больше {Properties.Settings.Default.PasswordMaxLength} символов";
                        break;
                    case PasswordValidationError.TooShort:
                        ViewModel.SaltValidationError = $"Пароль должен содержать не меньше {Properties.Settings.Default.PasswordMinLength} символов";
                        break;
                    case PasswordValidationError.RegexNotMatched:
                        ViewModel.SaltValidationError = $"Пароль должен удовлетворять рег. выражению {Properties.Settings.Default.PasswordRegex}";
                        break;
                    case PasswordValidationError.Ok:
                        ViewModel.SaltValidationError = "";
                        break;
                }

                ViewModel.Salt = lastPassword;
                System.Media.SystemSounds.Exclamation.Play();
            }
            return false;
        }



        // Click
        // -------------------------------------------------------------------
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settings = new();
            if (settings.ShowDialog() == true)
                ViewModel.IsPasswordRequired = Properties.Settings.Default.IsPasswordRequired;
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow about = new();
            about.ShowDialog();
        }

        private void Examples_Click(object sender, RoutedEventArgs e)
        {
            ExamplesWindow examples = new();
            if (examples.ShowDialog() == true)
            {
                ViewModel.SetStringHasher();
                ViewModel.CompareHashHex = examples.SelectedItem.Hash;
                ViewModel.Input = examples.SelectedItem.Message;
            }
        }

        private void SetFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new() { CheckFileExists = true };
            if (openFileDialog.ShowDialog() == true)
            {
                ViewModel.SetFileHasher(openFileDialog.FileName);
            }
        }

        private void Run_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckPassword()) 
                return;

            ViewModel.IsEnabled = false;

            Task.Run(() => {
                ViewModel.CalcHash();
                if (ViewModel.CompareHashHex != null)
                    ViewModel.CompareResult = ViewModel.CompareHashHex.ToLower() == ViewModel.HexHash; 

            }).ContinueWith(t => Dispatcher.Invoke(() =>
                ViewModel.IsEnabled = true       
            ));
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SetStringHasher();
            ViewModel.CompareResult = null;
        }

        private void LoadHash_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = "Текстовые файлы|*.txt|Любой файл|*.*",
                Title = "Загрузить контрольную сумму"
            };
            if (dialog.ShowDialog() == true)
            {
                ViewModel.IsEnabled = false;
                Task.Run(() => Dispatcher.Invoke(() =>
                {              
                    ViewModel.CompareHashHex = Utility.FileIO.ReadHashFile(dialog.FileName);
                    ViewModel.IsEnabled = true;
                }));
            }
        }

        private void SaveHash_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog()
            {
                CheckPathExists = true,
                Filter = "Текстовые файлы|*.txt",
                Title = "Сохранить контрольную сумму",
                FileName = FileIO.GenerateHashFileName(ViewModel.Input, ViewModel.IsFileHasher)
            };
            if (dialog.ShowDialog() == true)
            {
                ViewModel.IsEnabled = false;
                Task.Run(() => Dispatcher.Invoke(() =>
                {
                    FileIO.WriteHashFile(ViewModel.HexHash, dialog.FileName);
                    ViewModel.IsEnabled = true;
                }));
            }
        }

        private void CopyHash_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.HexHash == "")
                System.Media.SystemSounds.Asterisk.Play();
            else
            {
                Clipboard.SetText(ViewModel.HexHash);
                MessageBox.Show("Значение хеша скопировано в буффер обмена", "Скопировано");
            }
        }



        // Input
        // -------------------------------------------------------------------
        private void Input_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.HexHash = "";
        }

        private void CompareHash_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Any(ch => !(char.IsDigit(ch) || char.ToLower(ch) >= 'a' && char.ToLower(ch) <= 'f')))
            {
                e.Handled = true;
                System.Media.SystemSounds.Exclamation.Play();
            }
        }

        private void Password_TextChanged(object sender, TextChangedEventArgs e)
        {
            //CheckPassword(true);
        }
    }
}
