using Application.Common.Interfaces.Auth;
using Application.Common.InterFaces.DbContext;
using Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Auth
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IAdminDbContext DbContext;

        public TokenService(IConfiguration configuration, IAdminDbContext dbContext)
        {
            _configuration = configuration;
            this.DbContext = dbContext;
        }
        public async Task<List<Access>> GetUserAccesses(int userId)
        {
            var userAccesses = await (
                               from ur in DbContext.User
                               where ur.Id == userId
                               from ra in ur.Roles
                               join a in DbContext.Role on ra.Id equals a.Id
                               select a.Accesses
                                     ).SelectMany(x => x).Distinct().ToListAsync();

            var extraAccesses = await (
                                from ua in DbContext.UserAccess
                                where ua.User.Id == userId
                                join a in DbContext.Access on ua.AccessId equals a.Id
                                select ua
                                       ).Include(x => x.Access).ToListAsync();

            foreach (var extraAccess in extraAccesses)
            {
                if (extraAccess.IsAdded)
                {
                    if (!userAccesses.Any(x => x.Id == extraAccess.AccessId))
                    {
                        userAccesses.Add(extraAccess.Access);
                    }
                }
                else
                {
                    if (userAccesses.Any(x => x.Id == extraAccess.AccessId))
                    {
                        userAccesses.Remove(userAccesses.Where(x => x.Id == extraAccess.AccessId).Single());
                    }
                }
            }

            return userAccesses;
        }
        public async Task<CreateTokenResponse> CreateToken(CreateTokenRequest request, DateTime ExpireDateTime, ClaimsIdentity claimsIdentity = null)
        {
            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("Auth:Secret"));

            var claims = new ClaimsIdentity();
            if (claimsIdentity == null)
            {
                claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, request.UserName));
                claims.AddClaim(new Claim(ClaimTypes.Name, request.Name));
                claims.AddClaim(new Claim(ClaimTypes.Sid, request.UserId.ToString()));
         
            }


            var adminSettings = await DbContext.AdminSetting.FirstOrDefaultAsync();

            if (ExpireDateTime == null || ExpireDateTime == DateTime.MinValue)
            {
                ExpireDateTime = DateTime.Now.AddMinutes(adminSettings.UserSessionMaxTime);
            }


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = ExpireDateTime,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new CreateTokenResponse()
            {
                Token = tokenHandler.WriteToken(token),
               
            };
        }
        public async Task<string> CreateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }


        public async Task<List<Access>> GetUserAccesses(string userId)
        {
            try
            {
                int result = 0;
                int.TryParse(userId, out result);
                if (result != 0)
                {
                    return await GetUserAccesses(result);
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}
