
using System.Text.RegularExpressions;

namespace MD4_app.Utility.PasswordValidation
{
    internal enum PasswordValidationError
    {
        Ok, TooShort, NoCyryllic, NoLatin, NoDigits, NoUppercase, NoSpecial
    }
}
