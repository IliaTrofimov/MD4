using System;
using System.Linq;

namespace MD4_app.Utility.PasswordValidation
{
    internal class PasswordSymbolsRestriction
    {
        public bool MustHaveCyryllicSymbols { get; set; }
        public bool MustHaveLatinSymbols { get; set; }
        public bool MustHaveDigits { get; set; }
        public bool MustHaveUpperCase { get; set; }
        public bool MustHaveSpecialSymbols { get; set; }
        public int MinLength { get; set; } = -1;


        public (PasswordValidationError, string) CheckPassword(string password)
        {
            if (password.Length < MinLength)
                return (PasswordValidationError.TooShort, GetMessage(PasswordValidationError.TooShort));

            else if (MustHaveCyryllicSymbols && !password.Any(ch => "ЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮ".Contains(char.ToUpper(ch))))
                return (PasswordValidationError.NoCyryllic, GetMessage(PasswordValidationError.NoCyryllic));

            else if (MustHaveLatinSymbols && !password.Any(ch => "QWERTYUIOPASDFGHJKLZXCVBNM".Contains(char.ToUpper(ch))))
                return (PasswordValidationError.NoLatin, GetMessage(PasswordValidationError.NoLatin));

            else if (MustHaveDigits && !password.Any(ch => char.IsDigit(ch)))
                return (PasswordValidationError.NoDigits, GetMessage(PasswordValidationError.NoDigits));

            else if (MustHaveUpperCase && !password.Any(ch => char.IsUpper(ch)))
                return (PasswordValidationError.NoUppercase, GetMessage(PasswordValidationError.NoUppercase));

            else if (MustHaveSpecialSymbols && !password.Any(ch => char.IsPunctuation(ch)))
                return (PasswordValidationError.NoSpecial, GetMessage(PasswordValidationError.NoSpecial));

            return (PasswordValidationError.Ok, GetMessage(PasswordValidationError.Ok));
        }

        private string GetMessage(PasswordValidationError error) => error switch
        {
            PasswordValidationError.Ok => "",
            PasswordValidationError.TooShort => $"Парольная фраза должна содержать как минимум {MinLength} символов",
            PasswordValidationError.NoDigits => "Парольная фраза должна содержать хотя бы одна цифру",
            PasswordValidationError.NoLatin => "Парольная фраза должна содержать хотя бы одна латинску букву",
            PasswordValidationError.NoCyryllic => "Парольная фраза должна содержать хотя бы одну кириллическую букву",
            PasswordValidationError.NoSpecial => "Парольная фраза должна содержать хотя бы один спец. символ из списка !\"#%&'()*,-./:;?@[\\]_{}",
            PasswordValidationError.NoUppercase => "Парольная фраза должна содержать хотя бы одну заглавную букву",
            _ => "Неизвестная ошибка"
        };
    }
}
