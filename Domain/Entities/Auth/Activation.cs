using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;

namespace Domain.Entities.Auth
{
    [Table("Activation")]
    public class Activation:BaseEntity
    {
        
        public long UserId { get; set; }
        public int ActivationCode { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime ExpireTime { get; set; }
        public int Status { get; set; }
        public string IpAddress { get; set; }
        public User User { get; set; }
    }
}
