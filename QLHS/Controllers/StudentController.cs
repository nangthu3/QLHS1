using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using QLHS.Models;
using Microsoft.EntityFrameworkCore;

namespace QLHS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly StudentContext _context;

        Student[] students = new Student[]
        {
            new Student{Id = 1, Name = "Nguyen Van A", Gender= 1, Birthday = DateTime.Now, Address="Ha Noi" },
            new Student{Id = 2, Name = "Nguyen Van B", Gender= 1, Birthday = DateTime.Now, Address="Ha Nam" },
            new Student{Id = 3, Name = "Nguyen Van C", Gender= 1, Birthday = DateTime.Now, Address="Nam Dinh" },
            new Student{Id = 4, Name = "Nguyen Thi D", Gender= 0, Birthday = DateTime.Now, Address="Ninh Binh" },
            new Student{Id = 5, Name = "Nguyen Thi E", Gender= 0, Birthday = DateTime.Now, Address="Ha Giang" },
            new Student{Id = 6, Name = "Nguyen Thi F", Gender= 0, Birthday = DateTime.Now, Address="Thanh Hoa" },
            new Student{Id = 7, Name = "Le Thi G", Gender= 0, Birthday = DateTime.Now, Address="Nghe An" },
            new Student{Id = 8, Name = "Tran Van H", Gender= 1, Birthday = DateTime.Now, Address="Thai Binh" },
            new Student{Id = 9, Name = "Tran Van I", Gender= 1, Birthday = DateTime.Now, Address="Hai Duong" },
            new Student{Id = 10, Name = "Tran Thi K", Gender= 0, Birthday = DateTime.Now, Address="Quang Ninh" }
        };

        public StudentController(StudentContext context)
        {
            _context = context;
            if (_context.StudentItems.Count() == 0)
            {
                //_context.Add(
                //    new Student { Id = 1, Name = "Nguyen Van A", Gender = 1, Birthday = DateTime.Now, Address = "Ha Nam" }
                //    );
                _context.AddRange(students.ToList());
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudent()
        {
            return await _context.StudentItems.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudentById(int Id)
        {
            var student = await _context.StudentItems.FindAsync(Id);

            if (student == null)
            {
                return NotFound();
            }
            return student;
        }

        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            _context.StudentItems.Add(student);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetStudentById), new { id = student.Id }, student);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutStudent(long id, Student student)
        {
            if (id != student.Id)
            {
                return BadRequest();
            }
            _context.Entry(student).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(long id)
        {
            var student = await _context.StudentItems.FindAsync(id);
            if (student == null) return NotFound();
            _context.StudentItems.Remove(student);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}