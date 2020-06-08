using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Tools.Utils.Extensions
{
    public static class StringExtensions
    {
        public static string TrimJson(this string value)
        {
            if (value.IsNullOrEmpty()) return value;

            return Regex.Replace(value, "(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+", "$1");
        }

        #region "  Validation  "

        public static bool IsValidCPF(this string CPF)
        {
            try
            {
                CPF = CPF.ClearStrings();

                if (CPF.IsNullOrWhiteSpace() || CPF.Length != 11)
                    return false;

                for (int i = 0; i < CPF.Length; i++)
                    Convert.ToInt32(CPF[i].ToString());

                if (CPF == "00000000000" || CPF == "11111111111" || CPF == "22222222222" || CPF == "33333333333" || CPF == "44444444444" || CPF == "55555555555" || CPF == "66666666666" || CPF == "77777777777" || CPF == "88888888888" || CPF == "99999999999")
                    return false;

                int[] a = new int[11];
                int b = 0;
                int c = 10;
                int x = 0;

                for (int i = 0; i < 9; i++)
                {
                    a[i] = Convert.ToInt32(CPF[i].ToString());

                    b += (a[i] * c);
                    c--;
                }


                x = b % 11;

                if (x < 2)
                    a[9] = 0;
                else
                    a[9] = 11 - x;

                b = 0;
                c = 11;

                for (int i = 0; i < 10; i++)
                {
                    b += (a[i] * c);
                    c--;
                }

                x = b % 11;

                if (x < 2)
                    a[10] = 0;
                else
                    a[10] = 11 - x;

                if ((Convert.ToInt32(CPF[9].ToString()) != a[9]) || (Convert.ToInt32(CPF[10].ToString()) != a[10]))
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidCNPJ(this string CNPJ)
        {
            try
            {
                CNPJ = CNPJ.ClearStrings();

                if (CNPJ.IsNullOrWhiteSpace() || CNPJ.Length != 14)
                    return false;

                for (int i = 0; i < 14; i++)
                    Convert.ToInt32(CNPJ[i].ToString());

                int[] a = new int[14];
                int b = 0;
                int[] c = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
                int x = 0;

                for (int i = 0; i < 12; i++)
                {
                    a[i] = Convert.ToInt32(CNPJ[i].ToString());
                    b += a[i] * c[i + 1];
                }

                x = b % 11;

                if (x < 2)
                    a[12] = 0;
                else
                    a[12] = 11 - x;


                b = 0;
                for (int j = 0; j < 13; j++)
                    b += (a[j] * c[j]);

                x = b % 11;

                if (x < 2)
                    a[13] = 0;
                else
                    a[13] = 11 - x;

                if ((Convert.ToInt32(CNPJ[12].ToString()) != a[12]) || (Convert.ToInt32(CNPJ[13].ToString()) != a[13]))
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidZipCode(this string cep, bool allowDash = true)
        {
            Match match;

            if (allowDash)
                match = Regex.Match(cep, "^[0-9]{5}-[0-9]{3}$");
            else
                match = Regex.Match(cep.ClearStrings(), "^[0-9]{8}$");

            return match.Success;
        }

        public static bool IsValidEmail(this string email)
        {
            if (email.IsNullOrWhiteSpace()) return false;

            Match match = Regex.Match(email, @"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$", RegexOptions.IgnoreCase);

            return match.Success;
        }

        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static bool IsNumeric(this string value)
        {
            Match match = Regex.Match(value, "^[0-9]*$");

            return match.Success;
        }

        public static bool IsUnicode(this string input)
        {
            const int MaxAnsiCode = 255;

            return input.Any(c => c > MaxAnsiCode);
        }

        public static bool HasUnicodeChar(this string input)
        {
            const int MaxAnsiCode = 255;
            return input.Any(c => c > MaxAnsiCode);
        }

        #endregion

        #region "  Strings Ajustment  "

        public static string ClearStrings(this string value)
        {
            if (value.IsNullOrEmpty()) return value;

            return value.Replace(".", string.Empty)
                        .Replace(",", string.Empty)
                        .Replace(":", string.Empty)
                        .Replace("/", string.Empty)
                        .Replace(@"\", string.Empty)
                        .Replace("\"", string.Empty)
                        .Replace("'", string.Empty)
                        .Replace("-", string.Empty);
        }


        public static string TakeAccentsOff(this string value)
        {
            if (value.IsNullOrEmpty()) return value;

            value = Regex.Replace(value, "[áàâãª]", "a");
            value = Regex.Replace(value, "[ÁÀÂÃ]", "A");
            value = Regex.Replace(value, "[éèê]", "e");
            value = Regex.Replace(value, "[ÉÈÊ]", "e");
            value = Regex.Replace(value, "[íìî]", "i");
            value = Regex.Replace(value, "[ÍÌÎ]", "I");
            value = Regex.Replace(value, "[óòôõº]", "o");
            value = Regex.Replace(value, "[ÓÒÔÕ]", "O");
            value = Regex.Replace(value, "[úùû]", "u");
            value = Regex.Replace(value, "[ÚÙÛ]", "U");
            value = Regex.Replace(value, "[ç]", "c");
            value = Regex.Replace(value, "[Ç]", "C");

            return value;
        }

        public static string URLEncode(this string url)
        {
            return HttpUtility.UrlEncode(url);
        }

        public static string URLDecode(this string url)
        {
            return HttpUtility.UrlDecode(url).Replace(" ", "+");
        }

        public static string Truncate(this string value, int maxLength)
        {
            if (value.IsNullOrWhiteSpace())
                return string.Empty;

            int length = value.Length > maxLength ? maxLength : value.Length;
            return value.Substring(0, length);
        }

        public static string MaskCNPJ(this string CNPJ)
        {
            if (CNPJ.IsNullOrEmpty() || CNPJ.Length < 14) return CNPJ;

            return string.Format("{0}.{1}.{2}/{3}-{4}",
                CNPJ.Substring(0, 2), CNPJ.Substring(2, 3), CNPJ.Substring(5, 3), CNPJ.Substring(8, 4), CNPJ.Substring(12, 2));
        }

        public static string MaskCPF(this string CPF)
        {
            if (CPF.IsNullOrEmpty() || CPF.Length < 11) return CPF;

            return string.Format("{0}.{1}.{2}-{3}",
                CPF.Substring(0, 3), CPF.Substring(3, 3), CPF.Substring(6, 3), CPF.Substring(9, 2));
        }

        public static string MaskCEP(this string CEP)
        {
            if (CEP.IsNullOrEmpty() || CEP.Length < 8) return CEP;

            return string.Format("{0}-{1}",
                CEP.Substring(0, 5), CEP.Substring(5));
        }

        public static int OnlyNumbers(this string value)
        {
            return Regex.Match(value, @"\d+").Value.AsInt();
        }
        public static string OnlyAlphanumeric(this string value)
        {
            return Regex.Replace(value, @"[^A-Za-z0-9 ]", string.Empty).Trim();
        }

        public static string ToFormat(this string value, params object[] values)
        {
            return string.Format(value, values);
        }

        public static string EncodeToUTF8(this string value)
        {
            byte[] bytes = Encoding.Default.GetBytes(value);
            return Encoding.UTF8.GetString(bytes);
        }

        #endregion
    }
}
