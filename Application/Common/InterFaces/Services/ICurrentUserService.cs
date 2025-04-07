using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Services
{
    public interface ICurrentUserService
    {
     
        string? UserRoleId { get; }

        Guid Key { get; }

        string? AlternativeToken { get; }

        string? UrlOwner { get; }

        string? NationalCode { get; }

        string NationalCodeOwner { get; }

        string RoleTypeOwner { get; }

        string Name { get; }

        string Username { get; }
    }
}
