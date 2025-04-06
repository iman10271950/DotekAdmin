using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Auth
{
    [Table("RoleAccess")]
    public class RoleAccess
    {
        public Role Role { get; set; }
        public long RoleId { get; set; }
        public Access Access { get; set; }
        public long AccessId { get; set; }

    }
}
