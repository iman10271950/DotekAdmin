using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Auth
{
    [Table("Access")]
    public class Access:BaseEntity
    {

        public string Name { get; set; }
        public int ParentID { get; set; }
        public int Priority { get; set; }
        public int ISMenu { get; set; }
        public int Status { get; set; }
        public string MenuUrl { get; set; }
        public string? Icon { get; set; }
        public bool HasSubMenu { get; set; }
        public List<Role> Roles { get; set; }
        public List<User> Users { get; set; }
    }
}
