using Microsoft.EntityFrameworkCore;
using Student_Scheduler.Models;

namespace Student_Scheduler.Data
{
    public class StudentDataContext : DbContext
    {
        public StudentDataContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Student> Students { get; set; }
    }
}
