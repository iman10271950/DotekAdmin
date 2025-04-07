using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Business.Common.ViewModel
{
    public class Password_VM
    {
        public string HashedPassword { get; set; }
        public byte[] Salt { get; set; }
    }
}
