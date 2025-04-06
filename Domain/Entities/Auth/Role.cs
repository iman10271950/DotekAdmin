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
    [Table("Roles")]
    public class Role:BaseEntity
    {       
        public string Name { get; set; }


        //navigation properties
        public List<User> Users { get; set; }
        public List<Access> Accesses { get; set; }
    }
}
