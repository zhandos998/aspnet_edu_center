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
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string adminRoleName = "admin";
            string teacherRoleName = "teacher";
            string studentRoleName = "student";

            string adminEmail = "admin@admin.com";
            string adminPassword = "123456";

            // добавляем роли
            Role adminRole = new Role { Id = 1, Name = adminRoleName };
            Role teacherRole = new Role { Id = 2, Name = teacherRoleName };
            Role studentRole = new Role { Id = 3, Name = studentRoleName };
            User adminUser = new User { Id = 1, Name = "Qairat Qairatovich", Email = adminEmail, Password = adminPassword, Role_id = adminRole.Id, Tel_num = "+77777777"};

            modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, teacherRole, studentRole });
            modelBuilder.Entity<User>().HasData(new User[] { adminUser });
            base.OnModelCreating(modelBuilder);
        }
    }
}