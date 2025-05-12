using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums.Auth;

namespace Application.Business.Auth.User.ViewModel
{
    public class UpdateUser_VM
    {
        public long Id { get; set; }
        public string Name { get; set; }
       
        public string LastName { get; set; }
        public string? CompanyName { get; set; }
       
        public string Username { get; set; }
      
        public string Password { get; set; }
  
        public int Status { get; set; }
       
        public string MobileNumber { get; set; }
     
        public string NationalCode { get; set; }
       
        public string BirthDate { get; set; }
     
        public int TwoStageLogin { get; set; }
       
        public UserType Type { get; set; }


        public UserRate UserRate { get; set; }
    }
}
