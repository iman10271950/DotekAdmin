using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Mapping;

namespace Application.Business.TransportVehicleBusiness.ViewModel
{
    public class TransportVehicle_VM
    {
        public long Id { get; set; }
        /// <summary>
        /// نام راننده
        /// </summary>
        public string DriverFirstName { get; set; }
        /// <summary>
        /// نام خانوادگی راننده
        /// </summary>
        public string DriverLastName { get; set; }
        /// <summary>
        /// شماره تماس راننده
        /// </summary>
        public string DriverPhoneNumber { get; set; }
        /// <summary>
        ///  پلاک ماشین
        /// </summary>
        public string VehiclePlateNumber { get; set; }

    }
}
