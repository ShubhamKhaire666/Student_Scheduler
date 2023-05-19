using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_Scheduler.Data;
using Student_Scheduler.Models;
using System.ComponentModel.DataAnnotations;

namespace Student_Scheduler.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : Controller
    {
        private readonly StudentDataContext studentDBContext;
        public StudentController(StudentDataContext dbContext)
        {
            studentDBContext = dbContext;
        }

        [HttpGet("GetStudents")]
        public IActionResult GetStudentsInfo()
        {
            var students = studentDBContext.Students.Select(s => new { s.ID, s.Email, s.PhoneNumber, s.Name }).ToList();
            return Ok(students);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetStudentInfo([FromRoute] Guid id)
        {
            var students = studentDBContext.Students
                .Where(s => s.ID == id)
                .Select(s => new { s.ID, s.Email, s.PhoneNumber, s.Name })
                .ToList();

            return Ok(students);
        }

        [HttpPost("Login")]
        public bool StudentLogin(StudentLogin studentLogin)
        {
            if(StudentLogin == null)
                return false;

            var studentTryingToLogin = studentDBContext.Students.FirstOrDefault(u => u.Email == studentLogin.Email
            && u.Password == studentLogin.Password);

            if (studentTryingToLogin == null)
                return false;

            return true;
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult StudentDelete([FromRoute] Guid id)
        {
            var student = studentDBContext.Students.Find(id);

            if (student == null)
                return NotFound();

            studentDBContext.Students.Remove(student);
            studentDBContext.SaveChanges();

            return (Ok(student));
        }

        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult StudentUpdate([FromRoute] Guid id, StudentUpdate studentUpdate)
        {
            var student = studentDBContext.Students.Find(id);

            if (student == null)
                return NotFound();

            student.Email = studentUpdate.Email;
            student.PhoneNumber = studentUpdate.PhoneNumber;
            student.Name = studentUpdate.Name;

            studentDBContext.SaveChanges();

            return (Ok(student));
        }

        [HttpPost("Signup")]
        public IActionResult StudentSignup(StudentSignup studentSignup)
        {
            if (studentSignup == null)
                return NotFound();

            var student = new Student()
            {
                ID = Guid.NewGuid(),
                Name = studentSignup.Name,
                Email = studentSignup.Email,
                PhoneNumber = studentSignup.PhoneNumber,
                Password = studentSignup.Password
            };

            studentDBContext.Add(student);
            studentDBContext.SaveChanges();

            return Ok(studentSignup);
        }

    }
}
