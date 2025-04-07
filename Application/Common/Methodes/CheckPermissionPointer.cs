using Application.Common.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Methodes
{
    public class CheckPermissionPointer : ICheckPermissionPointer
    {
        public CheckPermissionPointer()
        {
            Pointer = false;
        }
        public bool Pointer { get; set; }
    }
}
