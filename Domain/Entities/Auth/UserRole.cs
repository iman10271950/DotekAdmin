using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Auth
{
    [Table("UserRole")]
    public class UserRole
    {
        public long UserId { get; set; }
        public long RoleId { get; set; }


        //navigation properties
        public Role Role { get; set; }
        public User User { get; set; }
    }
}
