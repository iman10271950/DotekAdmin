using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ServiceLog
{
    public class ServiceLogEntity
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// نام فارسی موجودیت
        /// </summary>
        [Description("نام فارسی موجودیت")]
        public string PersianName { get; set; }
        /// <summary>
        /// نام انگلیسی موجودیت
        /// </summary>
        [Description("نام انگلیسی موجودیت")]
        public string EnglishName { get; set; }
        /// <summary>
        /// وضعیت
        /// </summary>
        [Description("وضعیت")]
        public int Status { get; set; }

        //Navigation Properties
        public virtual List<ServiceLog> ServiceLog { get; set; }
    }
}
