using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Extentions;
using Domain.Common;
using Domain.Entities.Auth;
using Domain.Enum;
using Domain.Enums.Auth;

namespace Application.Business.DotekUser.VieModel
{
    public class DotekUser_VM
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }

        public string MobileNumber { get; set; }

        public string NationalCode { get; set; }

        public string BirthDate { get; set; }

        public int Status { get; set; }
        public string StatusDesc { get => ((Domain.Enums.Auth.UserStatus)Status).GetDescription(); }
        public int Type { get; set; }
        public string? UserTypeDesc { get; set; }

        public int UserRate { get; set; }
        public string? UserRateDesc { get; set; }

        public List<BaseEnum_VM>? AccesseId { get; set; }
        public List<BaseEnum_VM>? RolesId { get; set; }
    }
}
