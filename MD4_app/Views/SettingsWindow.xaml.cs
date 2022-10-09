using MD4_app.ViewModels;
using System.Windows;

namespace MD4_app.Views
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsVeiwModel ViewModel;

        public SettingsWindow()
        {
            InitializeComponent();
            ViewModel = new()
            {
                PasswordMaxLength = Properties.Settings.Default.PasswordMaxLength,
                PasswordMinLength = Properties.Settings.Default.PasswordMinLength,
                PasswordRegex = Properties.Settings.Default.PasswordRegex,
                IsPasswordRequired = Properties.Settings.Default.IsPasswordRequired
            };
            DataContext = ViewModel;
        }
   

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.PasswordMaxLength < ViewModel.PasswordMinLength)
            {
                ViewModel.ValidationErrorString = "Недопустимые значения минимальной и максимальной длины пароля";
                System.Media.SystemSounds.Exclamation.Play();
                return;
            }

            DialogResult = true;
            Properties.Settings.Default.PasswordMaxLength = ViewModel.PasswordMaxLength;
            Properties.Settings.Default.PasswordMinLength = ViewModel.PasswordMinLength;
            Properties.Settings.Default.PasswordRegex = ViewModel.PasswordRegex;
            Properties.Settings.Default.IsPasswordRequired = ViewModel.IsPasswordRequired;
            Properties.Settings.Default.Save();
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

    }
}
