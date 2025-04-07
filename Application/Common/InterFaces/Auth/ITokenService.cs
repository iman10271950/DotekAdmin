using Application.Common.Auth;
using Domain.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Auth
{
    public interface ITokenService
    {
        public Task<List<Access>> GetUserAccesses(int userId);
        public Task<List<Access>> GetUserAccesses(string userId);
        public Task<CreateTokenResponse> CreateToken(CreateTokenRequest request, DateTime ExpireDateTime, ClaimsIdentity claimsIdentity = null);

        public Task<string> CreateRefreshToken();

    }
}
