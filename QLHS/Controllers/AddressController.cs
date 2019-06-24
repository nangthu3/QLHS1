using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLHS.Models;

namespace QLHS.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private List<City> cities;
        private List<District> districts;
        private XmlSerializer citySerializer, districtSerializer;

        public AddressController()
        {
            citySerializer = new XmlSerializer(typeof(List<City>));
            districtSerializer = new XmlSerializer(typeof(List<District>));
            GetCitiesAndDistricts();
        }

        private void GetCitiesAndDistricts()
        {
            FileStream stream = new FileStream("App_Data/cities.xml", FileMode.Open);
            cities = (List<City>)citySerializer.Deserialize(stream);
            stream.Close();

            stream = new FileStream("App_Data/districts.xml", FileMode.Open);
            districts = (List<District>)districtSerializer.Deserialize(stream);
            stream.Close();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<String>> GetAddress(long id)
        {
            var district = districts.FirstOrDefault(d => d.Id == id);
            if (district != null)
            {
                var city = cities.FirstOrDefault(c => c.Id == district.CitiId);
                if (city != null)
                {
                    return district.Name + ", " + city.Name;
                }
            }
            return NotFound();
        }

        [HttpGet("city")]
        public async Task<ActionResult<IEnumerable< City>>> GetCities(long id)
        {
            return cities;
        }

        [HttpGet("city/{id}")]
        public async Task<ActionResult<City>> GetCity(long id)
        {
            var city = cities.FirstOrDefault(c => c.Id == id);
            if (city != null) return city;
            return NotFound();
        }

        [HttpGet("districtbycity/{id}")]
        public async Task<ActionResult<IEnumerable<District>>> GetDistrictByCityId(long id)
        {
            return districts.FindAll(d=> d.CitiId == id);
        }

        [HttpGet("district/{id}")]
        public async Task<ActionResult<District>> GetDistrict(long id)
        {
            var district = districts.FirstOrDefault(d => d.Id == id);
            if (district != null) return district;
            return NotFound();
        }

    }
}