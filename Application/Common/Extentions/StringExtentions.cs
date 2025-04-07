using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Extentions
{
    public static class StringExtentions
    {
        public static long GetNumbers(this string input)
        {
            long number = -1000;
            try
            {
                number = long.Parse(new string(input.Where(c => char.IsDigit(c)).ToArray()));
            }
            catch (Exception) { }
            return number;
        }

        public static int GetInt32(this string input)
        {
            int number = 0;
            try
            {
                if (!string.IsNullOrEmpty(input))
                    number = int.Parse(new string(input.Where(c => char.IsDigit(c)).ToArray()));
            }
            catch (Exception) { }
            return number;
        }

        public static long GetLong(this string input)
        {
            long number = 0;
            try
            {
                if (!string.IsNullOrEmpty(input))
                    number = long.Parse(new string(input.Where(c => char.IsDigit(c)).ToArray()));
            }
            catch (Exception) { }
            return number;
        }

        public static string ConvertStandardPersian(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            return str.Replace("ﮎ", "ک").Replace("ﮏ", "ک").Replace("ﮐ", "ک").Replace("ﮑ", "ک").Replace("ك", "ک").Replace("ي", "ی");
        }

    }
}
