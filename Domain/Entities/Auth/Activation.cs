using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Auth
{
    [Table("Activation")]
    public class Activation
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ActivationCode { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime ExpireTime { get; set; }
        public int Status { get; set; }
        public string IpAddress { get; set; }
        public User User { get; set; }
    }
}
