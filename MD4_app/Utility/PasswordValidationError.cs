using System.Text.RegularExpressions;

namespace MD4_app.Utility
{
    internal enum PasswordValidationError
    {
        Ok, TooShort, TooLagre, RegexNotMatched
    }

    internal static class PasswordValidation
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

        public static string GetValidationString(PasswordValidationError error)
        {
            return error switch
            {
                PasswordValidationError.TooLagre => $"Пароль должен содержать не больше {Properties.Settings.Default.PasswordMaxLength} символов",
                PasswordValidationError.TooShort => $"Пароль должен содержать не меньше {Properties.Settings.Default.PasswordMinLength} символов",
                PasswordValidationError.RegexNotMatched => $"Пароль должен удовлетворять рег. выражению {Properties.Settings.Default.PasswordRegex}",
                PasswordValidationError.Ok => "",
                _ => "Неизвестная ошибка",
            };
        }
    }

}
