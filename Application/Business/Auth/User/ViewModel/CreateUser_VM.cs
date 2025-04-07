using Application.Common.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Auth;
using Domain.Enums.Auth;

namespace Application.Business.Auth.User.ViewModel
{
    public class CreateUser_VM : IMapFrom<Domain.Entities.Auth.User>
    {
        
        /// <summary>
        /// رمز عبور
        /// </summary>
        public string Password { get; set; }


        /// <summary>
        /// شماره موبایل
        /// </summary>
        public string MobileNumber { get; set; }

        /// <summary>
        /// کد ملی
        /// </summary>
        public string NationalCode { get; set; }

        /// <summary>
        /// تاریخ تولد
        /// </summary>
        public string BirthDate { get; set; }


        /// <summary>
        /// تایید رمز عبور
        /// </summary>
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// نوع کاربر
        /// </summary>
        public UserType UserType { get; set; }
    }

}
