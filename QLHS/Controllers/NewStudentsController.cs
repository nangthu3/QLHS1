using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using QLHS.Models;
using Microsoft.EntityFrameworkCore;
using System.Xml.Serialization;
using System.IO;
using Microsoft.AspNetCore.Cors;
using QLHS.Business;

namespace QLHS.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class NewStudentsController : ControllerBase
    {
        private XmlSerializer xmlSerializer;
        private List<NewStudent> students;
        private List<StudentInfo> studentInfos;
        private AddressBusiness addressBusiness;

        public NewStudentsController()
        {
            addressBusiness = new AddressBusiness();
            xmlSerializer = new XmlSerializer(typeof(List<Student>));
            if (studentInfos == null || studentInfos.Count == 0)
            {
                studentInfos = GetAllStudentInfos();
            }
        }

        public List<StudentInfo> GetAllStudentInfos()
        {
            GetAllStudents();
            List<StudentInfo> studentInfo = new List<StudentInfo>();
            foreach (var student in students)
            {
                AddressItem addressItem = addressBusiness.GetAddressItem(student.AddressId);
                studentInfo.Add(new StudentInfo { Student = student, Address = addressItem });
            }
            return studentInfo;
        }

        public List<NewStudent> GetAllStudents()
        {
            try
            {
                FileStream stream = System.IO.File.OpenRead("App_Data/new_students.xml");
                students = (List<NewStudent>)xmlSerializer.Deserialize(stream);
                stream.Close();
            }
            catch (Exception)
            {
                students = new List<NewStudent>();
            }

            return students;
        }

        private void SaveStudents()
        {
            if (students != null && students.Count > 0)
            {
                FileStream stream = new FileStream("App_Data/new_students.xml", FileMode.Create);
                xmlSerializer.Serialize(stream, students);
                stream.Close();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentInfo>>> GetStudent()
        {
            return GetAllStudentInfos();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StudentInfo>> GetStudentById(long id)
        {
            GetAllStudents();
            var student = studentInfos.Find(st => st.Student.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            return student;
        }

        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(NewStudent student)
        {
            GetAllStudents();
            if (students == null || students.Count == 0)
            {
                student.Id = 0;
            }
            else { student.Id = students.Last().Id + 1; }
            students.Add(student);
            SaveStudents();
            return CreatedAtAction(nameof(GetStudentById), new { id = student.Id }, student);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutStudent(long id, NewStudent student)
        {
            if (id != student.Id)
            {
                return BadRequest();
            }
            GetAllStudents();

            int index = students.FindIndex(s => s.Id == id);
            if (index >= 0)
            {
                students[index] = student;
                SaveStudents();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(long id)
        {
            GetAllStudents();
            var student = students.Find(st => st.Id == id);
            if (student == null) return NotFound();
            students.Remove(student);
            SaveStudents();
            return NoContent();
        }
    }
}