using Microsoft.EntityFrameworkCore;
using aspnet_edu_center.Models;

namespace aspnet_edu_center.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Group_type> Group_types { get; set; }
        public DbSet<Group_User> Group_Users { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Tests> Tests { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Timetable> Timetables { get; set; }
        public DbSet<StudDocument> StudDocuments { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group_User>().HasKey(c => new { c.User_Id, c.Group_Id });
            modelBuilder.Entity<Grade>().HasKey(c => new { c.User_id, c.Date });
            modelBuilder.Entity<Attendance>().HasKey(c => new { c.User_id, c.Date });
            //modelBuilder.Entity<Answer>().HasNoKey();
            base.OnModelCreating(modelBuilder);
        }
    }
}