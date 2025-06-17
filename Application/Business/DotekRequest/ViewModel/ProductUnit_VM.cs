
using Application.Common.Mapping;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Business.DotekRequest.ViewModel
{
    public class ProductUnit_VM 
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string MobileNumber { get; set; }
        public string PostalCode { get; set; }
        public int Status { get; set; }



    }
}
