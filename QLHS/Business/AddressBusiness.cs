using QLHS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QLHS.Business
{
    public class AddressBusiness
    {
        private List<City> cities;
        private List<District> districts;
        private XmlSerializer citySerializer, districtSerializer;

        public AddressBusiness()
        {
            citySerializer = new XmlSerializer(typeof(List<City>));
            districtSerializer = new XmlSerializer(typeof(List<District>));
            GetCitiesAndDistricts();
        }

        public void GetCitiesAndDistricts()
        {
            GetCities();
            GetDistricts();
        }

        public List<City> GetCities()
        {
            FileStream stream = new FileStream("App_Data/cities.xml", FileMode.Open);
            cities = (List<City>)citySerializer.Deserialize(stream);
            stream.Close();
            return cities;
        }

        public List<District> GetDistricts()
        {
            FileStream stream = new FileStream("App_Data/districts.xml", FileMode.Open);
            districts = (List<District>)districtSerializer.Deserialize(stream);
            stream.Close();
            return districts;
        }

        public List<City> GetAllCities()
        {
            return cities;
        }

        public City GetCityById(long id)
        {
            return cities.FirstOrDefault(c => c.Id == id);
        }

        public string GetAddressName(long addressId)
        {
            var district = districts.FirstOrDefault(d => d.Id == addressId);
            if (district != null)
            {
                var city = cities.FirstOrDefault(c => c.Id == district.CitiId);
                if (city != null)
                {
                    return district.Name + ", " + city.Name;
                }
            }
            return "";
        }

        public void SaveCities()
        {
            if (cities == null) cities = new List<City>();
            FileStream stream = new FileStream("App_Data/cities.xml", FileMode.Create);
            citySerializer.Serialize(stream, cities);
            stream.Close();
        }

        public void InsertCity(City city)
        {
            GetCities();
            cities.Add(city);
            SaveCities();
        }

        public void InsertCities(List<City> cities)
        {
            GetCities();
            this.cities.AddRange(cities);
            SaveCities();
        }


        public void DeleteCity(long id)
        {
            GetCities();
            var city = cities.FirstOrDefault(c => c.Id == id);
            if (city != null)
            {
                cities.Remove(city);
                SaveCities();
            }
        }

        public void DeleteCity(City city)
        {
            GetCities();
            if (cities.Contains(city))
            {
                cities.Remove(city);
                SaveCities();
            }
        }

        public void DeleteAllCity()
        {
            cities.Clear();
            SaveCities();
        }

        public void SaveDisricts()
        {
            if (districts == null) districts = new List<District>();
            FileStream stream = new FileStream("App_Data/districts.xml", FileMode.Create);
            districtSerializer.Serialize(stream, districts);
            stream.Close();
        }

        public District GetDistrictById(long id)
        {
            return districts.FirstOrDefault(d => d.Id == id);
        }

        public List<District> GetAllDistrictOfCity(long cityId)
        {
            return districts.FindAll(d => d.CitiId == cityId);
        }

        public void InsertDistricts(District district)
        {
            GetDistricts();
            districts.Add(district);
            SaveDisricts();
        }

        public void DeleteDistrict(long id)
        {
            GetDistricts();
            var district = districts.FirstOrDefault(d => d.Id == id);
            if (district != null)
            {
                districts.Remove(district);
                SaveDisricts();
            }
        }

        public void DeleteDistrict(District district)
        {
            GetDistricts();
            if (districts.Contains(district))
            {
                districts.Remove(district);
                SaveDisricts();
            }
        }

        public void DeleteAllDistricts()
        {
            districts.Clear();
            SaveDisricts();
        }

        public AddressItem GetAddressItem(long id)
        {
            AddressItem item = new AddressItem ();
            item.DistrictId = id;
            var district = GetDistrictById(id);
            if (district!= null)
            {
                item.DistrictName = district.Name;
                item.Cityname = GetCityById(district.CitiId).Name;
            }
            return item;
        }
    }
}
