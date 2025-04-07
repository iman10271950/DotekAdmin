using Application.Common.Extentions;
using Domain.Enums.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Business.Common.ViewModel
{
    public class AuthenticatedResponse_VM
    {
        /// <summary>
        /// نام کاربر
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// نام خانوادگی کاربر
        /// </summary>
        public string LastName { get; set; }
        public string Token { get; set; }
        public UserType UserTypeId { get; set; }
       
        public string RefreshToken { get; set; }
        /// <summary>
        /// زمان انقضای توکن اصلی
        /// </summary>
        public int SessionExpireTime { get; set; }
        public string? UserTypeDesc { get; set; }
    }
}
