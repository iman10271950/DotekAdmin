using Application.Common.Interfaces.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Services
{
    public interface ICheckPermissionPointer
    {
        public bool Pointer { get; set; }
    }

}
