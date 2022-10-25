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

namespace MD4_app
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal HashGeneratorViewModel ViewModel { get; set; }
        private string lastPassword;
        private CancellationTokenSource? runCancellationTokenSource = null;
        private PasswordSymbolsRestriction PasswordRestrictions;

        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new HashGeneratorViewModel();
            DataContext = ViewModel;
            ViewModel.IsPasswordRequired = Properties.Settings.Default.IsPasswordRequired;
            lastPassword = ViewModel.Salt;

            PasswordRestrictions = new()
            {
                MustHaveUpperCase = Properties.Settings.Default.PasswordMustHaveUppercase,
                MustHaveCyryllicSymbols = Properties.Settings.Default.PasswordMustHaveCyryllic,
                MustHaveLatinSymbols = Properties.Settings.Default.PasswordMustHaveLatin,
                MustHaveDigits = Properties.Settings.Default.PasswordMustHaveDigits,
                MustHaveSpecialSymbols = Properties.Settings.Default.PasswordMustHaveSpecial,
                MinLength = Properties.Settings.Default.PasswordMinLength
            };


            Title = $"MD4 [process id: {Environment.ProcessId}]";
        }

        private bool CheckPassword(bool isSilent = false)
        {
            if (!Properties.Settings.Default.IsPasswordRequired)
                return true;

            (PasswordValidationError res, string msg) = PasswordRestrictions.CheckPassword(ViewModel.Salt);
            if (res == PasswordValidationError.Ok)
            {
                ViewModel.SaltValidationError = "";
                lastPassword = ViewModel.Salt;
                return true;
            }
            else if (!isSilent)
            {
                ViewModel.SaltValidationError = msg;
                System.Media.SystemSounds.Exclamation.Play();
            }
            return false;
        }

        private async Task RunHashing()
        {
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
        }



        // Click
        // -------------------------------------------------------------------
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settings = new(PasswordRestrictions);
            if (settings.ShowDialog() == true)
                ViewModel.IsPasswordRequired = Properties.Settings.Default.IsPasswordRequired;
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow about = new();
            about.ShowDialog();
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            string helpMessage = "Доступные действия:\n" +
                "-----------------------------------------------------------\n" +
                "• Для хеширования строки введите текст в поле ввода;\n" +
                "• Для хеширования файла назмите на сслыку 'ВЫБЕРИТЕ ФАЙЛ' в заголовке поля ввода или в меню 'ФАЙЛ';\n" +
                "• Чтобы сбросить выбор файла или очистить поле ввода нажмите 'ОЧИСТИТЬ';\n\n" +
                "• Для сохранения хеша в текстовый файл нажмите кнопку 'СОХРАНИТЬ' или 'СОХРАНИТЬ ХЕШ' в меню 'ФАЙЛ';\n" +
                "• Для копирвания хеша в буффер обмена нажмите кнопку 'КОПИРОВАТЬ';\n\n" +
                "• Чтобы сравнить хеш введённой строки (файла) введите 16-ричное значение сравниваемого хеша (или загрузите файл с хешем) в поле внизу окна;\n" +
                "• Для сравненеия используются файлы с хешами, которые программа создаёт при сохранении хеша;\n\n" +
                "• Чтобы добавить 'соль' к хешу включите в меню 'НАСТРОЙКИ' опцию 'ТРЕБОВАТЬ ПАРОЛЬ' и введите парольную фразу в поле справа внизу окна.";
            MessageBox.Show(helpMessage, "Помощь", MessageBoxButton.OK, MessageBoxImage.Question);
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
                try
                {
                    await RunHashing();
                }
                catch (Exception ex)
                {
                    ViewModel.IsEnabled = true;
                    System.Media.SystemSounds.Asterisk.Play();
                    MessageBox.Show(ex.Message, "Ошибка");
                }
            }
            else if (runCancellationTokenSource != null && !runCancellationTokenSource.IsCancellationRequested)
                runCancellationTokenSource.Cancel();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SetStringHasher();
            ViewModel.CompareResult = HashCompareResult.None;
        }

        private async void LoadHash_Click(object sender, RoutedEventArgs e)
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
                try
                {
                    await Task.Run(() => ViewModel.CompareHashHex = FileIO.ReadHashFile(dialog.FileName));
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

        private void CopyHash_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.HexHash == "")
                System.Media.SystemSounds.Asterisk.Play();
            else
            {
                Clipboard.SetText(ViewModel.HexHash);
                System.Media.SystemSounds.Hand.Play();
                MessageBox.Show("Значение хеша скопировано в буффер обмена", "Скопировано");
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

        private void Password_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckPassword(true);
        }



        // Closing
        // -------------------------------------------------------------------
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (runCancellationTokenSource != null && !runCancellationTokenSource.IsCancellationRequested)
                runCancellationTokenSource.Cancel();
        }
    }
}
