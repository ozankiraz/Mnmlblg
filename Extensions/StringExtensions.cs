using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Mnmlblg.Extensions
{
    public static class StringExtensions
    {
        public static string GenerateSlug(this string phrase)
        {
            var slug = phrase.RemoveDiacritics().ToLower();

            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", ""); // invalid chars          
            slug = Regex.Replace(slug, @"\s+", " ").Trim(); // convert multiple spaces into one space  
            //slug = slug.Substring(0, slug.Length <= 45 ? slug.Length : 45).Trim(); // cut and trim it  
            slug = Regex.Replace(slug, @"\s", "-"); // hyphens  

            return slug;
        }

        public static string RemoveDiacritics(this string text)
        {
            string stFormD = text.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }

            return (sb.ToString().Normalize(NormalizationForm.FormC));
        }
        
        public static string GetPreview(this string stringForPreview, int maxCharacters)
        {
            var length = stringForPreview.Length;
            var endPosition = length > maxCharacters ? maxCharacters : length;
            var preview = string.Format("{0}...", stringForPreview.Substring(0, endPosition));

            return preview.Contains("data:image") ? string.Empty : preview;
        }
    }
}