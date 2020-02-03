using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XT.BusinessService
{
    public static class StringUlti
    {
        public static string ToSafeUrl(this string s)
        {
            s = s.Convert_Chuoi_Khong_Dau();
            s = Regex.Replace(s, @"[^a-zA-Z0-9]", "-");
            s = Regex.Replace(s, @"-{2,}", "-");

            return s.ToLower();
            //return s.Replace("/", "_").Replace("+", "-").Replace(" ", "-").Replace(",", "").Replace(".", "");
        }

        public static string Convert_Chuoi_Khong_Dau(this string s)
        {
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string strFormD = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(strFormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }

        public static string Remove_Khoang_Trang(this string s)
        { 
            return s.Replace(" ", "");
        }

        public static string UpperCaseFirst(this string p)
        {
            if (string.IsNullOrEmpty(p))
            {
                return string.Empty;
            }

            return char.ToUpper(p[0]) + p.Substring(1);
        }

        public static string CutString(this string str, int length)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            return str.Substring(0, Math.Min(length, str.Length));
        }

        public static bool WordContains(this string p, string term)
        {
            var i = p.IndexOf(term);
            var j = i + term.Length;
            var last = p.Length - 1;
            if (i < 0)
                return false;
            if (i == 0 || !Char.IsLetter(p[i - 1]))
            {
                if (j <= last && !Char.IsLetter(p[j]))
                    return true;
            }

            return false;
        }

        public static bool WordPartialContains(this string p, string term)
        {
            var i = p.IndexOf(term);
            //var j = i + term.Length;
            //var last = p.Length - 1;
            if (i < 0)
                return false;
            if (i == 0 || !Char.IsLetter(p[i - 1]))
            {
                return true;
            }

            return false;
        }
    }
}