using Domain.Enums.Auth;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Auth
{
    public class DotekAuthorizeAttribute : AuthorizeAttribute
    {
        public DotekAuthorizeAttribute(AuthorizeEnum authorizePolicy)
        {
            Policy = ((int)authorizePolicy).ToString();
        }
    }
}
