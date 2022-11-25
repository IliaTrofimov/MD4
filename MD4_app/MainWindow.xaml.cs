using MD4_app.Utility;
using MD4_app.Utility.PasswordValidation;
using MD4_app.ViewModels;
using MD4_app.Views;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MD4_app
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal HashGeneratorViewModel ViewModel { get; set; }
        private CancellationTokenSource? runCancellationTokenSource = null;
        private PasswordSymbolsRestriction PasswordRestrictions;

        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new HashGeneratorViewModel();
            DataContext = ViewModel;
            ViewModel.IsPasswordRequired = Properties.Settings.Default.IsPasswordRequired;

            PasswordRestrictions = new()
            {
                MustHaveUpperCase = Properties.Settings.Default.PasswordMustHaveUppercase,
                MustHaveCyryllicSymbols = Properties.Settings.Default.PasswordMustHaveCyryllic,
                MustHaveLatinSymbols = Properties.Settings.Default.PasswordMustHaveLatin,
                MustHaveDigits = Properties.Settings.Default.PasswordMustHaveDigits,
                MustHaveSpecialSymbols = Properties.Settings.Default.PasswordMustHaveSpecial,
                MinLength = Properties.Settings.Default.PasswordMinLength
            };
        }

        private bool CheckPassword()
        {
            if (!ViewModel.IsPasswordRequired || !Properties.Settings.Default.IsRestrictionsEnabled)
            {
                ViewModel.SaltValidationError = "";
                return true;
            }

            (PasswordValidationError res, string msg) = PasswordRestrictions.CheckPassword(ViewModel.Salt);
       
            if (res == PasswordValidationError.Ok)
            {
                ViewModel.SaltValidationError = "";
                return true;
            }
            else 
            {
                ViewModel.SaltValidationError = msg;
                System.Media.SystemSounds.Exclamation.Play();
                return false;
            }
        }

        private async Task RunHashing()
        {
            ViewModel.HexHash = "";
            if (!CheckPassword())
                return;

            ViewModel.IsEnabled = false;
            runCancellationTokenSource = new CancellationTokenSource();

            await Task.Run(() =>
            {
                ViewModel.CalcHash();
                if (ViewModel.CompareHashHex != null)
                {
                    if (ViewModel.CompareHashHex.Length != 32)
                        ViewModel.CompareResult = HashCompareResult.WrongLength;
                    else
                        ViewModel.CompareResult = ViewModel.CompareHashHex == ViewModel.HexHash ? HashCompareResult.Equal : HashCompareResult.NotEqual;
                }
            }, runCancellationTokenSource.Token);
            ViewModel.IsEnabled = true;

            if (ViewModel.Input == "")
            {
                System.Media.SystemSounds.Hand.Play();
                MessageBox.Show("Была хеширована пустая строка", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information);
            }               
        }



        // Click
        // -------------------------------------------------------------------
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settings = new(PasswordRestrictions);
            settings.ShowDialog();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow about = new();
            about.ShowDialog();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Examples_Click(object sender, RoutedEventArgs e)
        {
            ExamplesWindow examples = new();
            if (examples.ShowDialog() == true)
            {
                ViewModel.SetStringHasher();
                ViewModel.CompareHashHex = examples.SelectedItem.Hash;
                ViewModel.Input = examples.SelectedItem.Message;
                ViewModel.CompareValue = null;
            }
        }

        private void SetFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new() { CheckFileExists = true, Title = "Выбрать файл для хеширования" };
            if (openFileDialog.ShowDialog() == true)
                ViewModel.SetFileHasher(openFileDialog.FileName);
        }

        private async void ToggleHashing_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.IsEnabled)
            {
                if (ViewModel.IsFileHasher && string.IsNullOrWhiteSpace(ViewModel.Input))
                {
                    ViewModel.IsEnabled = true;
                    System.Media.SystemSounds.Exclamation.Play();
                    MessageBox.Show("Необходимо выбрать файл", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                try
                {
                    await RunHashing();
                }
                catch (Exception ex)
                {
                    ViewModel.IsEnabled = true;
                    System.Media.SystemSounds.Asterisk.Play();
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (runCancellationTokenSource != null && !runCancellationTokenSource.IsCancellationRequested)
            {
                runCancellationTokenSource.Cancel();
                ViewModel.HexHash = "";
                ViewModel.IsEnabled = true;
            }
        }


        private async void SaveHash_Click(object sender, RoutedEventArgs e)
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
                try
                {
                    await Task.Run(() => FileIO.WriteHashFile(ViewModel.Hasher, dialog.FileName));
                }
                catch (Exception ex)
                {
                    System.Media.SystemSounds.Asterisk.Play();
                    MessageBox.Show(ex.Message, "Ошибка");
                }
                finally
                {
                    ViewModel.IsEnabled = true;
                }
            }
        }

        private async void SelectComparingFile_Click(object sender, RoutedEventArgs e)
        {
            CompareSelectionWindow compareWinodw = new();
            if (compareWinodw.ShowDialog() == true)
            {
                ViewModel.CompareHashHex = compareWinodw.ComparingHashValue;
                ViewModel.CompareValue = compareWinodw.ComparingFile;

                if (ViewModel.IsFileHasher && string.IsNullOrWhiteSpace(ViewModel.Input))
                {
                    ViewModel.IsEnabled = true;
                    System.Media.SystemSounds.Exclamation.Play();
                    MessageBox.Show("Нет данных для сравнения.\n\nВыберите файл или введите текст и нажмите\nВЫЧИСЛИТЬ ХЕШ для сравнения с загруженным хешем.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                if (ViewModel.CompareHashHex != null)
                    await RunHashing();
            }
        }


        // Input
        // -------------------------------------------------------------------
        private void Input_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.HexHash = "";
            ViewModel.CompareResult = HashCompareResult.None;
        }

        private void CompareHash_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ViewModel.CompareResult = HashCompareResult.None;

            if (e.Text.Any(ch => !(char.IsDigit(ch) || char.ToLower(ch) >= 'a' && char.ToLower(ch) <= 'f')))
            {
                e.Handled = true;
                System.Media.SystemSounds.Exclamation.Play();
                ViewModel.CompareResult = HashCompareResult.WrongSymbol;
            }
        }

        private void Clear_Comparision(object sender, RoutedEventArgs e)
        {
            ViewModel.CompareHashHex = null;
            ViewModel.CompareValue = null;
            ViewModel.CompareResult = HashCompareResult.None;
        }

        private void Clear_Text(object sender, RoutedEventArgs e)
        {
            ViewModel.SetStringHasher();
            ViewModel.CompareResult = HashCompareResult.None;
        }
        private void PasswordChanged(object sender, RoutedEventArgs e)
        {
            ViewModel.Salt = ((PasswordBox)sender).Password;
        }

        private void PasswordUnchecked(object sender, RoutedEventArgs e)
        {
            pswdbox_Salt.Password = "";
        }



        // Closing
        // -------------------------------------------------------------------
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (runCancellationTokenSource != null && !runCancellationTokenSource.IsCancellationRequested)
                runCancellationTokenSource.Cancel();

            Properties.Settings.Default.IsPasswordRequired = ViewModel.IsPasswordRequired;
            Properties.Settings.Default.Save();
        }

    }
}
