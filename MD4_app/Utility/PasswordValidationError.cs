using System.Text.RegularExpressions;

namespace MD4_app.Utility
{
    public enum PasswordValidationError
    {
        Ok, TooShort, TooLagre, RegexNotMatched
    }

    public static class PasswordValidation
    {
        public static PasswordValidationError Validate(string password)
        {
            if (Properties.Settings.Default.IsPasswordRequired)
            {
                int len = password.Length;
                if (len > Properties.Settings.Default.PasswordMaxLength)
                    return PasswordValidationError.TooLagre;
                else if (len < Properties.Settings.Default.PasswordMinLength)
                    return PasswordValidationError.TooShort;
                if (Properties.Settings.Default.PasswordRegex != "" && !Regex.IsMatch(password, Properties.Settings.Default.PasswordRegex))
                    return PasswordValidationError.RegexNotMatched;
            }

            return PasswordValidationError.Ok;
        }
    }

}
