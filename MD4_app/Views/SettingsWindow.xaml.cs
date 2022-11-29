using MD4_app.Utility.PasswordValidation;
using MD4_app.ViewModels;
using System.Windows;

namespace MD4_app.Views
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        internal SettingsVeiwModel ViewModel;
        internal readonly PasswordSymbolsRestriction restrictions;

        internal SettingsWindow(PasswordSymbolsRestriction restriction)
        {
            InitializeComponent();
            ViewModel = new(restriction, Properties.Settings.Default.IsRestrictionsEnabled);
            DataContext = ViewModel;
            restrictions = restriction;
        }
   

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Properties.Settings.Default.IsRestrictionsEnabled = ViewModel.IsRestrictionEnabled;
            Properties.Settings.Default.PasswordMustHaveLatin = ViewModel.MustHaveLatinSymbols;
            Properties.Settings.Default.PasswordMustHaveCyryllic = ViewModel.MustHaveCyryllicSymbols;
            Properties.Settings.Default.PasswordMustHaveUppercase = ViewModel.MustHaveUpperCase;
            Properties.Settings.Default.PasswordMustHaveSpecial = ViewModel.MustHaveSpecialSymbols;
            Properties.Settings.Default.PasswordMinLength = ViewModel.PasswordMinLength;
            Properties.Settings.Default.Save();

            restrictions.MustHaveLatinSymbols = ViewModel.MustHaveLatinSymbols;
            restrictions.MustHaveUpperCase = ViewModel.MustHaveUpperCase;
            restrictions.MustHaveDigits = ViewModel.MustHaveDigits;
            restrictions.MustHaveSpecialSymbols = ViewModel.MustHaveSpecialSymbols;
            restrictions.MustHaveCyryllicSymbols = ViewModel.MustHaveCyryllicSymbols;
            restrictions.MinLength = ViewModel.PasswordMinLength;

            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

    }
}
