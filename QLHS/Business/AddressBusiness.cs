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
            FileStream stream = new FileStream("App_Data/cities.xml", FileMode.Open);
            cities = (List<City>)citySerializer.Deserialize(stream);
            stream.Close();

            stream = new FileStream("App_Data/districts.xml", FileMode.Open);
            districts = (List<District>)districtSerializer.Deserialize(stream);
            stream.Close();
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

        }

        public void InsertCity()
        {

        }

        public void InsertCities(List<City> cities)
        {

        }


        public void DeleteCity(long id)
        {

        }

        public void DeleteCity(City city)
        {

        }

        public void DeleteAllCity()
        {

        }

        public void SaveDisricts()
        {

        }

        public District GetDistrictById(long id)
        {
            return districts.FirstOrDefault(d => d.Id == id);
        }

        public List<District> GetAllDistrictOfCity(long cityId)
        {
            return districts.FindAll(d => d.CitiId == cityId);
        }

        public void InsertDistricts()
        {

        }

        public void DeleteDistrict(long id)
        {

        }

        public void DeleteDistrict(District district)
        {

        }

        public void DeleteAllDistricts()
        {

        }

    }
}
