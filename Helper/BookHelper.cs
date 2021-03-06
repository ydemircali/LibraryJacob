using System;
using System.Linq;

namespace LibraryJacob.Helper
{
    public static class BookHelper
    {
        public static string  GenreUpperCase(string str)
        {
            string result = "";

            if (!string.IsNullOrEmpty(str))
            {
                var frags = str.Split('_');
                for (var i=0; i < frags.Length; i++) {
                    frags[i] = frags[i].First().ToString().ToUpper() + frags[i].Substring(1);
                }

                result = string.Join(" ", frags);
            }
            
            return result;
        }

        public static string TitleNameSurname(string str)
        {
            string result = "";

            if (!string.IsNullOrEmpty(str))
            {
                var frags = str.Split(',');
                for (var i = frags.Length-1; i >= 0; i--) {
                    result += " " + frags[i];
                }
            }
            
            return result;
        }
        
    }
    
}