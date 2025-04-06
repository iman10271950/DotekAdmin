using Domain.Common;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.AdminSetting
{
    public class AdminSetting
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// زمان انقضای توکن jwt به دقیقه
        /// </summary>
        public int UserJWTTokenExpireTime { get; set; }
        /// <summary>
        /// زمان انقضای نشست کاربر به دقیقه
        /// </summary>
        public int UserSessionMaxTime { get; set; }
        /// <summary>
        /// تعداد نشست های مجاز کاربر
        /// </summary>
        public int MaxUserSessions { get; set; }
    }
}
