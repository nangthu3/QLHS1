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
    public class StudentsController : ControllerBase
    {
        private StudentBusiness studentBusiness;

        public StudentsController()
        {
            studentBusiness = new StudentBusiness();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentInfo>>> GetStudent()
        {
            return studentBusiness.GetAllStudentInfos();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StudentInfo>> GetStudentById(long id)
        {
            var student = studentBusiness.GetAllStudentInfos().Find(st => st.Student.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            return student;
        }

        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            studentBusiness.InsertStudent(student);
            return CreatedAtAction(nameof(GetStudentById), new { id = student.Id }, student);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutStudent(long id, Student student)
        {
            if (id != student.Id)
            {
                return BadRequest();
            }
            studentBusiness.UpdateStudent(student);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(long id)
        {
            studentBusiness.DeleteStudent(id);
            return NoContent();
        }
    }
}