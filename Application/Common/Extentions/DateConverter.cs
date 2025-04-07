using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Application.Common.Extentions
{
    

    public static class DateConverter
    {
        /// <summary>
        /// تبدیل تاریخ میلادی به شمسی
        /// </summary>
        /// <param name="date">تاریخ میلادی</param>
        /// <returns>تاریخ شمسی به صورت رشته</returns>
        public static string ToShamsi(this DateTime date)
        {
            PersianCalendar persianCalendar = new PersianCalendar();
            int year = persianCalendar.GetYear(date);
            int month = persianCalendar.GetMonth(date);
            int day = persianCalendar.GetDayOfMonth(date);

            return $"{year:0000}/{month:00}/{day:00}";
        }

        /// <summary>
        /// تبدیل تاریخ شمسی به میلادی
        /// </summary>
        /// <param name="shamsiDate">تاریخ شمسی به صورت رشته (yyyy/MM/dd)</param>
        /// <returns>تاریخ میلادی</returns>
        public static DateTime ToGregorian(this string shamsiDate)
        {
            string[] parts = shamsiDate.Split('/');
            if (parts.Length != 3)
                throw new FormatException("تاریخ شمسی باید به فرمت yyyy/MM/dd باشد.");

            int year = int.Parse(parts[0]);
            int month = int.Parse(parts[1]);
            int day = int.Parse(parts[2]);

            PersianCalendar persianCalendar = new PersianCalendar();
            return persianCalendar.ToDateTime(year, month, day, 0, 0, 0, 0);
        }
    }

}
