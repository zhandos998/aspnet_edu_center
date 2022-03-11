using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using aspnet_edu_center.Models;
using aspnet_edu_center.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using Microsoft.AspNet.Identity;

namespace aspnet_edu_center.Controllers.Teacher
{
    [Authorize(Roles = "2")]
    public class TeacherController : Controller
    {
        private ApplicationContext _context;
        public TeacherController(ApplicationContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ViewMyGroup()
        {
            var users = _context.Users.
            Join(_context.Group_Users,
            a => a.Id,
            b => b.User_Id,
            (a, b) => new
            {
                Id = a.Id,
                Name = a.Name,
                Email = a.Email,
                Tel_num = a.Tel_num,
                Group_Id = b.Group_Id
            }
            ).
            Join(_context.Groups,
            a => a.Group_Id,
            b => b.Id,
            (a, b) => new
            {
                Id = a.Id,
                Name = a.Name,
                Email = a.Email,
                Tel_num = a.Tel_num,
                Group_type = b.Group_type,
                Supervisor_id = b.Supervisor_id
            }
            ).
            //Join(_context.Group_types,
            //a => a.Group_type,
            //b => b.Id,
            //(a, b) => new
            //{
            //    Id = a.Id,
            //    Name = a.Name,
            //    Email = a.Email,
            //    Tel_num = a.Tel_num,
            //    Group_type = b.Name,
            //    Supervisor_id = a.Supervisor_id
            //}
            //).
            Where(a => a.Supervisor_id == int.Parse(User.Identity.Name)).ToList();
            List<Dictionary<string, object>> usersList = new List<Dictionary<string, object>>();

            foreach (var user in users)
            {
                usersList.Add(new Dictionary<string, object>() {
                    { "Id", user.Id },
                    { "Name", user.Name },
                    { "Email", user.Email },
                    { "Tel_num", user.Tel_num },
                    { "Group_type", user.Group_type },
                    { "Supervisor_id", user.Supervisor_id },
                    { "user_id", User.Identity.Name }
                });
            }
            return View(usersList);
        }

        [HttpPost]
        public async Task<IActionResult> AttendanceStudent(int id)
        {
            Attendance attendance = _context.Attendances.FirstOrDefault(u => u.User_id == id && u.Date == DateTime.Now.Date);
            if (attendance == null)
            {
                attendance = new Attendance { User_id = id, Date = DateTime.Now.Date, Camed = true };
                _context.Attendances.Add(attendance);
                await _context.SaveChangesAsync();
            }
            else
            {
                attendance.Camed = !attendance.Camed;
                _context.Entry(attendance).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            Console.WriteLine("Success");
            return Ok();
        }
        public IActionResult ViewGroups()
        {
            var users = _context.Groups.
            Where(a => a.Supervisor_id == int.Parse(User.Identity.Name)).ToList();
            List<Dictionary<string, object>> usersList = new List<Dictionary<string, object>>();

            foreach (var user in users)
            {
                usersList.Add(new Dictionary<string, object>() {
                    { "Id", user.Id },
                    { "Name", user.Name },
                    { "Group_type", user.Group_type },
                    { "Supervisor_id", user.Supervisor_id },
                    { "user_id", User.Identity.Name }
                });
            }
            return View(usersList);
        }

        public IActionResult DetailsGroup(int id)
        {
            ViewBag.Group_id = id;
            var users = (from Groups in _context.Groups
                         join Group_Users in _context.Group_Users on Groups.Id equals Group_Users.Group_Id
                         join Users in _context.Users on Group_Users.User_Id equals Users.Id
                         join Attendances in _context.Attendances on Users.Id equals Attendances.User_id into ps
                         from Attendances in ps.DefaultIfEmpty()
                         where Groups.Id == id
                         select new
                         {
                             Id = Users.Id,
                             Name = Users.Name,
                             Email = Users.Email,
                             Tel_num = Users.Tel_num,
                             Camed = Attendances.Camed,
                         }).ToList();
            List<Dictionary<string, object>> usersList = new List<Dictionary<string, object>>();

            foreach (var user in users)
            {
                usersList.Add(new Dictionary<string, object>() {
                    { "Id", user.Id },
                    { "Name", user.Name },
                    { "Email", user.Email },
                    { "Tel_num", user.Tel_num },
                    { "Camed", user.Camed }
                });
            }
            return View(usersList);
        }
        public IActionResult DetailsUser(int id,int group_id)
        {
            ViewBag.Group_id = group_id;
            ViewBag.User_Name = _context.Users.First().Name;
            ViewBag.User_id = id;
            var users = _context.Users.
            Where(a => a.Id == id).
            Join(_context.Grades,
            a => a.Id,
            b => b.User_id,
            (a, b) => new
            {
                Grades = b.Grades,
                Date = b.Date,
            }
            )
            .ToList();
            List<Dictionary<string, object>> usersList = new List<Dictionary<string, object>>();

            foreach (var user in users)
            {
                usersList.Add(new Dictionary<string, object>() {
                    { "Grades", user.Grades },
                    { "Date", user.Date }
                });
            }
            return View(usersList);
        }
        [HttpPost]
        public async Task<IActionResult> CreateGrade(int id,int grade,int user_id) {
            Grade Grade = new Grade {User_id=user_id, Grades = grade,Date=DateTime.Now};

            _context.Grades.Add(Grade);
            await _context.SaveChangesAsync();
            return RedirectToAction("DetailsGroup", new { id = id });
        }

        public IActionResult CreateTest()
        {
            return View();
        }
        


    }
}
