using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ServiceLog
{
    public class ServiceLog
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// نام فارسی سرویس
        /// </summary>
        [Description("نام فارسی سرویس")]
        public string PersianName { get; set; }
        /// <summary>
        /// نام انگلیسی سرویس
        /// </summary>
        [Description("نام انگلیسی سرویس")]
        public string EnglishName { get; set; }
        /// <summary>
        /// آیدی سرویس
        /// </summary>
        [Description("آیدی سرویس")]
        public int ServiceId { get; set; }
        /// <summary>
        /// آیدی متد
        /// </summary>
        [Description("آیدی متد")]
        public int MethodId { get; set; }
        /// <summary>
        /// آیدی والد
        /// </summary>
        [Description("آیدی والد")]
        public int ParentId { get; set; }
        /// <summary>
        /// آیدی موجودیت
        /// </summary>
        [Description("آیدی موجودیت")]
        public int EntityId { get; set; }
        /// <summary>
        /// آیدی موجودیت
        /// </summary>
        [Description("آیدی موجودیت")]
        public int Status { get; set; }
        /// <summary>
        /// آیدی نوع لاگ
        /// </summary>
        [Description("آیدی نوع لاگ")]
        public int LogTypeId { get; set; }

        /// <summary>
        ///الویت نمایش منو ها 
        /// </summary>
        [Description("الویت")]
        public int? Priority { get; set; }

        /// <summary>
        ///نام فیلد پوینتر 
        /// </summary>
        [Description("نام فیلد پوینتر")]
        public string? PointerIdName { get; set; }

        //Navigation Properties
        public virtual ServiceLogEntity ServiceLogEntity { get; set; }
    }
}
