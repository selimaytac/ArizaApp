using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ArizaApp.Helpers
{
    public static class StringHelper
    {
        public static string TurkishToEnglishToUpper(this string text)
        {
            return String.Join("", text.ToUpper().Normalize(NormalizationForm.FormD)
                .Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark));
        }
    }
}