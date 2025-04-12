using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Auth;
using Domain.Enums.Auth;

namespace Application.Business.DotekRequest.DotekUser.VieModel
{
    public class DotekUser_VM
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
        public byte[] Salt { get; set; }
        public int Status { get; set; }

        public string MobileNumber { get; set; }

        public string NationalCode { get; set; }

        public string BirthDate { get; set; }


        public int Type { get; set; }
        public string? UserTypeDesc { get; set; }

        public int UserRate { get; set; }
        public string? UserRateDesc { get; set; }

        public List<Access>? Accesses { get; set; }
        public List<Role>? Roles { get; set; }
    }
}
