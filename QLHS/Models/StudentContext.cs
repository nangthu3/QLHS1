using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QLHS.Models
{
    public class StudentContext : DbContext
    {
        private XmlSerializer xmlSerializer;
        public StudentContext(DbContextOptions<StudentContext> options)
            : base(options)
        {
            xmlSerializer = new XmlSerializer(typeof(List<Student>));
        }

        public DbSet<Student> StudentItems { get; set; }
    }
}