using Application.Business.DotekRequest.ViewModel;
using Application.Common.InterFaces.Services;
using Microsoft.Extensions.Configuration;

namespace Application.Common.Methodes
{
    public class AuthHelper: IAuthHelper
    {
        private readonly IConfiguration _configuration;

        public AuthHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public OtherServicesAuth_VM GetDefaultAuth()
        {
            var auth = new OtherServicesAuth_VM();
            _configuration.GetSection("AuthDefaults").Bind(auth);
            return auth;
        }
    }

}
