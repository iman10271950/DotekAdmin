using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Business.DotekRequest.ViewModel;

namespace Application.Common.InterFaces.Services
{
    public interface IAuthHelper
    {
        public OtherServicesAuth_VM GetDefaultAuth();
    }
}
