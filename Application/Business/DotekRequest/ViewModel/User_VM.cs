using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.BaseEntities;
using Application.Common.Extentions;
using Application.Common.Mapping;
using Domain.Entities.Auth;

using Domain.Enums.Auth;


namespace Application.Business.DotekRequest.ViewModel
{
    public class User_VM
    {
        public long Id { get; set; }
        public string Name { get; set; }
     
        public string LastName { get; set; }

        public string Username { get; set; }
  
      
        public int Status { get; set; }

        public string MobileNumber { get; set; }
 
        public string NationalCode { get; set; }
  
        public DateTime BirthDate { get; set; }
        public string ShamsiBirthDate { get; set; }

        public UserType Type { get; set; }
        public string? UserTypeDesc { get; set; }

        public UserRate UserRate { get; set; }
        public string? UserRateDesc { get; set; }

        public List<BaseEnum_VM>? AccesseId { get; set; }
    
      
    }
}
