using Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.DateTime
{
    public class DateTimeService : IDateTime
    {
        System.DateTime IDateTime.Now => System.DateTime.Now;
    }

}
