using Application.Common.Mapping;
using Domain.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Business.Common.ViewModel
{
    public class Session_VM : IMapFrom<Session>
    {
        public Guid Id { get; set; }
        public string BrowserName { get; set; }
        public string BrowserVersion { get; set; }
        public string OperatingSystem { get; set; }
    }
}
