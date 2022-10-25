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

            else if (MustHaveDigits && !password.Any(ch => char.IsUpper(ch)))
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
            PasswordValidationError.TooShort => $"Требуется минимум {MinLength} символов",
            PasswordValidationError.NoDigits => "Требуется хотя бы одна цифра",
            PasswordValidationError.NoLatin => "Требуется хотя бы одна латинская буква",
            PasswordValidationError.NoCyryllic => "Требуется хотя бы одна кириллическая буква",
            PasswordValidationError.NoSpecial => "Требуется хотя бы один спец. символ !?@#$%^&*()[]{}<>~.,/|\\;:",
            PasswordValidationError.NoUppercase => "Требуется хотя бы одна заглавная буква",
            _ => "Неизвестная ошибка"
        };
    }
}
