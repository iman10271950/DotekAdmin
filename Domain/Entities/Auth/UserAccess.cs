using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Auth
{
    [Table("UserAccess")]
    public class UserAccess
    {
        public User User { get; set; }
        public long UserId { get; set; }
        public Access Access { get; set; }
        public long AccessId { get; set; }
        public bool IsAdded { get; set; }
    }
}
