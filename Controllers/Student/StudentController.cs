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
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace aspnet_edu_center.Controllers.Student
{
    [Authorize(Roles = "3")]
    public class StudentController : Controller
    {
        private ApplicationContext _context;
        IWebHostEnvironment _appEnvironment;
        public StudentController(ApplicationContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ViewTimetable()
        {
            var users = _context.Group_Users
            .Where(a => a.User_Id == int.Parse(User.Identity.Name))
            .Join(_context.Groups,
            a => a.Group_Id,
            b => b.Id,
            (a, b) => new
            {
                Id = b.Id,
                Name = b.Name,
                Group_type = b.Group_type,
                Supervisor_id = b.Supervisor_id,
            }
            )
            .Join(_context.Timetables,
            a => a.Id,
            b => b.Group_id,
            (a, b) => new
            {
                Id = a.Id,
                Name = a.Name,
                Group_type = a.Group_type,
                Supervisor_id = a.Supervisor_id,
                Week_day = b.Week_day,
                Time = b.Time,
            }
            )
            .Join(_context.Users,
            a => a.Supervisor_id,
            b => b.Id,
            (a, b) => new
            {
                Id = a.Id,
                Group_Name = a.Name,
                Group_type = a.Group_type,
                Supervisor_Name = b.Name,
                Week_day = a.Week_day,
                Time = a.Time,
            }
            )
            .Join(_context.Group_types,
            a => a.Group_type,
            b => b.Id,
            (a, b) => new
            {
                Id = a.Id,
                Group_Name = a.Group_Name,
                Group_type = b.Name,
                Supervisor_Name = a.Supervisor_Name,
                Week_day = a.Week_day,
                Time = a.Time,
            }
            )
            .OrderBy(a => a.Week_day)
            .ToList();
            List<Dictionary<string, object>> usersList = new List<Dictionary<string, object>>();
            foreach (var user in users)
            {
                string week = "";
                switch (user.Week_day)
                {
                    case 1: week = "Дүйсенбі"; break;
                    case 2: week = "Сейсенбі"; break;
                    case 3: week = "Сәрсенбі"; break;
                    case 4: week = "Бейсенбі"; break;
                    case 5: week = "Жұма"; break;
                    case 6: week = "Сенбі"; break;
                    case 7: week = "Жексенбі"; break;
                }
                usersList.Add(new Dictionary<string, object>() {
                    { "Id", user.Id },
                    { "Group_Name", user.Group_Name },
                    { "Supervisor_Name", user.Supervisor_Name },
                    { "Week_day", week },
                    { "Group_type", user.Group_type },
                    { "Time", user.Time },
                });
            }
            return View(usersList);
        }
        public ActionResult DetailsGroup(int id)
        {
            ViewBag.Group_id = id;
            return View(_context.Documents.Where(x => x.Group_id == id));
        }
        public IActionResult DownloadDocument(int id)
        {
            string path = _context.Documents.Find(id).Url;
            string file_path = _appEnvironment.WebRootPath + path;
            string file_type = "application/octet-stream";
            string file_name = _context.Documents.Find(id).Doc_name;
            return PhysicalFile(file_path, file_type, file_name);
        }
        public ActionResult ViewGrades(int id)
        {
            var users = _context.Grades
            .Where(a => a.User_id == int.Parse(User.Identity.Name));
            List<Dictionary<string, object>> usersList = new List<Dictionary<string, object>>();
            foreach (var user in users)
            {
                usersList.Add(new Dictionary<string, object>() {
                    { "Grades", user.Grades },
                    { "Date", user.Date.ToShortDateString() },
                });
            }
            return View(usersList);
        }
        public ActionResult ViewAttendance(int id)
        {
            var users = _context.Attendances
            .Where(a => a.User_id == int.Parse(User.Identity.Name))
            .ToList();
            List<Dictionary<string, object>> usersList = new List<Dictionary<string, object>>();
            foreach (var user in users)
            {
                usersList.Add(new Dictionary<string, object>() {
                    { "Camed", user.Camed },
                    { "Date", user.Date.ToShortDateString() },
                });
            }
            return View(usersList);
        }
        public ActionResult AddDocument(int id, int group_id)
        {
            ViewBag.Document_id = id;
            ViewBag.Group_id = group_id;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddDocument(IFormFile Url, int Document_id, int group_id, string Name)
        {
            if (Url != null)
            {
                // путь к папке Files
                string path = "/StudFiles/" + Url.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await Url.CopyToAsync(fileStream);
                }
                StudDocument file = new StudDocument {  Url = path, Document_id = Document_id, Doc_name = Url.FileName, Student_id = int.Parse(User.Identity.Name), created_at = DateTime.Now.Date };
                _context.StudDocuments.Add(file);
                _context.SaveChanges();
            }

            return RedirectToAction("DetailsGroup", new { id = group_id });
        }



    }
}
