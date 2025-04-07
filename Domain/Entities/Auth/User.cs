using Domain.Common;
using Domain.Enums.Auth;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Auth
{
    [Table("Users")]
    public class User:BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
        public string? CompanyName { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public byte[] Salt { get; set; }      
        public int Status { get; set; }
        [Required]
        public string MobileNumber { get; set; }
        [Required]
        public string NationalCode { get; set; }
        [Required]
        public string BirthDate { get; set; }
        [Required]
        public int TwoStageLogin { get; set; }
        [Required]
        public UserType Type { get; set; }


        public UserRate UserRate { get; set; }

        //navigation props
        public List<Role> Roles { get; set; }
        public List<Access> Accesses { get; set; }
        public List<Session> Sessions { get; set; }
        public List<Activation> Activation { get; set; }
      

    }
}
