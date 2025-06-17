
using Application.Common.BaseEntities;
using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Extentions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            DescriptionAttribute attribute = field?.GetCustomAttribute<DescriptionAttribute>();
            return attribute?.Description ?? value.ToString();
        }
        public static List<BaseEnum_VM> ToBaseEnumList<T>(this T enumType) where T : Enum
        {
            var enumValues = Enum.GetValues(typeof(T));
            var enumList = new List<BaseEnum_VM>();

            foreach (var value in enumValues)
            {
                enumList.Add(new BaseEnum_VM
                {
                    Id = (int)value, // مقدار عددی Enum
                    Name = value.ToString() // نام Enum
                });
            }

            return enumList;
        }
    }
}
