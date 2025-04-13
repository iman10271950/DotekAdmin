using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum OtherPropertyFieldsType
    {
        /// <summary>
        /// داده متنی
        /// </summary>
        [Description("داده متنی")]
        Text = 1,

        /// <summary>
        /// داده عددی
        /// </summary>
        [Description("داده عددی")]
        Numeric = 2,

        /// <summary>
        /// داده منطقی
        /// </summary>
        [Description("داده منطقی")]
        Boolean = 3,

        /// <summary>
        /// داده تاریخی
        /// </summary>
        [Description("داده تاریخی")]
        DateTime = 4,

        /// <summary>
        /// داده اعشاری
        /// </summary>
        [Description("داده اعشاری")]
        Decimal = 5,

        /// <summary>
        /// داده دودویی
        /// </summary>
        [Description("داده دودویی")]
        Binary = 6

    }
}
