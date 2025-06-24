using Domain.Entities.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Application.Common.Attributes
{
    public class DotekLogAttribute: Attribute
    {
        public AdminMethods methods;
        public AdminServices services;

        public DotekLogAttribute(AdminServices services, AdminMethods methods)
        {
            this.services = services;
            this.methods = methods;
        }
    }
}
