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

namespace QLHS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private XmlSerializer xmlSerializer;
        private List<Student> students;
        public StudentsController(StudentContext context)
        {
            xmlSerializer = new XmlSerializer(typeof(List<Student>));
            if (students == null || students.Count == 0)
            {
                students = getAllStudents();
            }
        }

        public List<Student> getAllStudents()
        {
            FileStream stream = System.IO.File.OpenRead("App_Data/students.xml");
            students = (List<Student>)xmlSerializer.Deserialize(stream);
            return students;
        }

        private void SaveStudents()
        {
            if (students == null || students.Count == 0)
            {
                FileStream stream = System.IO.File.OpenWrite("App_Data/students.xml");
                xmlSerializer.Serialize(stream, students);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudent()
        {
            return getAllStudents();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudentById(int id)
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
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            students = getAllStudents();
            students.Add(student);
            SaveStudents();
            return CreatedAtAction(nameof(GetStudentById), new { id = student.Id }, student);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutStudent(long id, Student student)
        {
            if (id != student.Id)
            {
                return BadRequest();
            }
            students = getAllStudents();
            //students.Where(s => s.Id == id).;
            Student st = students.Find(s => s.Id == id);
            if (st != null)
            {
                st = student;
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