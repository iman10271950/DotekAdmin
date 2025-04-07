using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Auth
{
    public class CreateTokenRequest
    {
        public long UserId { get; }
        public string UserName { get; }
        public string Name { get; }
        public string PersonTypeId { get; set; }
     

        public CreateTokenRequest(long userId, string userName, string name, string? personTypeId = null)
        {
            UserId = userId;
            UserName = userName;
            Name = name;
            PersonTypeId = personTypeId;
         
        }
    }
}
