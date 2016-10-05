using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Mnmlblg.Extensions
{
    public static class StringExtensions
    {
        public static string GenerateSlug(this string phrase)
        {
            var slug = phrase.RemoveAccent().ToLower();

            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", ""); // invalid chars          
            slug = Regex.Replace(slug, @"\s+", " ").Trim(); // convert multiple spaces into one space  
            //slug = slug.Substring(0, slug.Length <= 45 ? slug.Length : 45).Trim(); // cut and trim it  
            slug = Regex.Replace(slug, @"\s", "-"); // hyphens  

            return slug;
        }

        public static string RemoveAccent(this string txt)
        {
            var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return Encoding.ASCII.GetString(bytes);
        }

        public static string GenerateGravatarUrl(string email)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.  
            var md5Hasher = MD5.Create();

            // Convert the input string to a byte array and compute the hash.  
            var data = md5Hasher.ComputeHash(Encoding.GetEncoding(0).GetBytes(email.Trim().ToLower()));

            // Create a new Stringbuilder to collect the bytes  
            // and create a string.  
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data  
            // and format each one as a hexadecimal string.  
            foreach (var t in data)
            {
                sBuilder.Append(t.ToString("x2"));
            }

            return string.Format("http://www.gravatar.com/avatar/{0}?s=200", sBuilder.ToString());   // Return gravatar url for email
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