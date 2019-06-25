using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QLHS.Models
{
    public class AddressItem
    {
        public long DistrictId { get; set; }
        public long CityId { get; set; }
        public string DistrictName { get; set; }
        public string Cityname { get; set; }
    }
}
