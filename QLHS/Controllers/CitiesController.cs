using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using QLHS.Models;
using System.IO;
using System.Xml.Serialization;

namespace QLHS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private List<City> cities;
        private List<District> districts;
        private XmlSerializer cSerializer, dSerializer;

        public CitiesController()
        {
            cSerializer = new XmlSerializer(typeof(List<City>));
            dSerializer = new XmlSerializer(typeof(List<District>));
            GetCities();
        }

        [HttpGet]
        public ActionResult<IEnumerable<City>> GetCity()
        {
            return cities;
        }

        public void GetCities()
        {
            cities = new List<City>();
            districts = new List<District>();
            JObject jObject = JObject.Parse(System.IO.File.ReadAllText("App_Data/json.json"));
            if (jObject != null && jObject.Count > 0)
            {
                IList<string> keys = jObject.Properties().Select(p => p.Name).ToList();
                foreach (var key in keys)
                {
                    JObject childJObjec = jObject.Value<JObject>(key);
                    String cityName = childJObjec.Value<String>("name");
                    JObject districtObject = childJObjec.Value<JObject>("districts");
                    cities.Add(new City { Id = long.Parse(key), Name = cityName });
                    if (districtObject != null && districtObject.Count > 0)
                    {
                        IList<string> districtKeys = districtObject.Properties().Select(p => p.Name).ToList();
                        foreach (var districtKey in districtKeys)
                        {
                            string districtName = districtObject.Value<string>(districtKey);
                            districts.Add(
                                new District { Id = long.Parse(districtKey), CitiId = long.Parse(key), Name = districtName }
                                );
                        }
                    }
                }
                FileStream stream = new FileStream("App_Data/cities.xml", FileMode.Create);
                cSerializer.Serialize(stream, cities);
                stream.Close();

                stream = new FileStream("App_Data/districts.xml", FileMode.Create);
                dSerializer.Serialize(stream, districts);
                stream.Close();
            }
        }

        [HttpGet("{id}")]
        public ActionResult<City> GetCityById(long id)
        {
            var city = cities.FirstOrDefault(c => c.Id == id);
            if (city == null)
            {
                return NotFound();
            }
            return Ok(city);
        }

        [HttpGet("dis/{id}")]
        public ActionResult<IEnumerable<District>> GetDistricFromCity(long id)
        {
            return districts.Where(d => d.CitiId == id).ToList();
        }
    }
}