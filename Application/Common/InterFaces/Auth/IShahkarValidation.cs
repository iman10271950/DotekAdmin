using Application.Business.Auth.User.ViewModel;
using Application.Common.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Auth
{
    public interface IShahkarValidation
    {
        public Task<bool> Validation(string PhoneNumber, string NationalCode);
        public Task<bool> ShabaValidation( string NationalCode,string BirthDate,string IBAN);
        public Task<BaseResult_VM<string>> PostCodeValidationAndGetAddress(string PostalCode);
        public Task<BaseResult_VM<NationalIdentity_VM>> NationalIdentityInquiry(string NationalCode,string ShamsiBirthDate);

    }
}
