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

namespace QLHS.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class NewStudentsController : ControllerBase
    {
        private XmlSerializer xmlSerializer;
        private List<NewStudent> students;
        public NewStudentsController()
        {
            xmlSerializer = new XmlSerializer(typeof(List<Student>));
            if (students == null || students.Count == 0)
            {
                students = getAllStudents();
            }
        }

        public List<NewStudent> getAllStudents()
        {
            FileStream stream = System.IO.File.OpenRead("App_Data/students.xml");
            students = (List<NewStudent>)xmlSerializer.Deserialize(stream);
            stream.Close();
            return students;
        }

        private void SaveStudents()
        {
            if (students != null && students.Count > 0)
            {
                FileStream stream = new FileStream("App_Data/students.xml", FileMode.Create);
                xmlSerializer.Serialize(stream, students);
                stream.Close();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NewStudent>>> GetStudent()
        {
            return getAllStudents();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NewStudent>> GetStudentById(long id)
        {
            students = getAllStudents();
            var student = students.Find(st => st.Id == id);

            if (student == null)
            {
                return NotFound();
            }
            return student;
        }

        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(NewStudent student)
        {
            students = getAllStudents();
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
            students = getAllStudents();

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
            students = getAllStudents();
            var student = students.Find(st => st.Id == id);
            if (student == null) return NotFound();
            students.Remove(student);
            SaveStudents();
            return NoContent();
        }
    }
}