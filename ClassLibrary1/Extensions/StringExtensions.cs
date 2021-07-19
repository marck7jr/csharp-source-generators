using System.Globalization;
using System.Linq;
using System.Text;

namespace ClassLibrary1.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveDiacritics(this string @string)
        {
            if (string.IsNullOrWhiteSpace(@string))
                return @string;

            @string = @string.Normalize(NormalizationForm.FormD);
            var chars = @string
                .Where(x => CharUnicodeInfo.GetUnicodeCategory(x) != UnicodeCategory.NonSpacingMark)
                .ToArray();

            return new string(chars).Normalize(NormalizationForm.FormC);
        }
    }
}
