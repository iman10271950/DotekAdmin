using Application.Business.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Auth
{
    public interface IPasswordService
    {
        public Password_VM HashPassword(string password, byte[] hasSalt);
        public bool ValidatePassword(string password, string correctHash);
    }
}
