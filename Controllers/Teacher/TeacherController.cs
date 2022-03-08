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

        public IActionResult ViewAttendance(int id)
        {
            DbSet<Attendance> attendanceSet = _context.Attendances;
            return View(attendanceSet);
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
            var users = _context.Groups.
            Where(a => a.Id == id).
            Join(_context.Group_Users,
            a => a.Id,
            b => b.Group_Id,
            (a, b) => new
            {
                User_Id = b.User_Id
            }
            ).
            Join(_context.Users,
            a => a.User_Id,
            b => b.Id,
            (b, a) => new
            {
                Id = a.Id,
                Name = a.Name,
                Email = a.Email,
                Tel_num = a.Tel_num
            }
            )
            .ToList();
            List<Dictionary<string, object>> usersList = new List<Dictionary<string, object>>();

            foreach (var user in users)
            {
                usersList.Add(new Dictionary<string, object>() {
                    { "Id", user.Id },
                    { "Name", user.Name },
                    { "Email", user.Email },
                    { "Tel_num", user.Tel_num }
                });
            }
            return View(usersList);
        }
        public IActionResult DetailsUser(int id)
        {
            ViewBag.Group_id = id;
            var users = _context.Groups.
            Where(a => a.Id == id).
            Join(_context.Group_Users,
            a => a.Id,
            b => b.Group_Id,
            (a, b) => new
            {
                User_Id = b.User_Id
            }
            ).
            Join(_context.Users,
            a => a.User_Id,
            b => b.Id,
            (b, a) => new
            {
                Id = a.Id,
                Name = a.Name,
                Email = a.Email,
                Tel_num = a.Tel_num
            }
            )
            .ToList();
            List<Dictionary<string, object>> usersList = new List<Dictionary<string, object>>();

            foreach (var user in users)
            {
                usersList.Add(new Dictionary<string, object>() {
                    { "Id", user.Id },
                    { "Name", user.Name },
                    { "Email", user.Email },
                    { "Tel_num", user.Tel_num }
                });
            }
            return View(usersList);
        }
    }
}
