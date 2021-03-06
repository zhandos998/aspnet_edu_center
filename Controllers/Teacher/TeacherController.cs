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

namespace aspnet_edu_center.Controllers.Teacher
{
    [Authorize(Roles = "2")]
    public class TeacherController : Controller
    {
        private ApplicationContext _context;
        IWebHostEnvironment _appEnvironment;
        public TeacherController(ApplicationContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
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
            return Ok();
        }
        public IActionResult ViewGroups()
        {
            var users = _context.Groups.
            Where(a => a.Supervisor_id == int.Parse(User.Identity.Name)).
            Join(_context.Group_types,
            a => a.Group_type,
            b => b.Id,
            (a, b) => new
            {
                Id = a.Id,
                Name = a.Name,
                Group_type = b.Name,
                Supervisor_id = a.Supervisor_id,
            }
            ).
            Join(_context.Users,
            a => a.Supervisor_id,
            b => b.Id,
            (a, b) => new
            {
                Id = a.Id,
                Name = a.Name,
                Group_type = a.Group_type,
                Supervisor_id = b.Name,
            }
            )
            .ToList();
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
            ViewData["Title"] = _context.Groups.Find(id).Name;
            var users = (from Groups in _context.Groups
                         join Group_Users in _context.Group_Users on Groups.Id equals Group_Users.Group_Id
                         join Users in _context.Users on Group_Users.User_Id equals Users.Id
                         join Attendances in _context.Attendances on Users.Id equals Attendances.User_id
                         into ps
                         from Attendances in ps.DefaultIfEmpty()
                         where Groups.Id == id //&& Attendances.Date == DateTime.Now.Date
                         select new
                         {
                             Id = Users.Id,
                             Name = Users.Name,
                             Email = Users.Email,
                             Tel_num = Users.Tel_num,
                             Camed = Attendances.Camed,
                             Date = Attendances.Date,
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
            ViewBag.User_Name = _context.Users.First(d=>d.Id == id).Name;
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
                    { "Date", user.Date.ToShortDateString() }
                });
            }
            var attens = _context.Users.
                Where(a => a.Id == id).
                Join(_context.Attendances,
                a => a.Id,
                b => b.User_id,
                (a, b) => new
                {
                    Date = b.Date,
                    Camed = b.Camed
                }).ToList();


            List<Dictionary<string, object>> obj = new List<Dictionary<string, object>>();

            foreach (var atten in attens)
            {
                obj.Add(new Dictionary<string, object>() {
                    { "Date", atten.Date.ToShortDateString() },
                    { "Camed", atten.Camed }
                });
            }
            ViewBag.Attendances = obj;
            //---------------------------------
            var tests = _context.Users_Tests.
                Where(a => a.User_id == id).
                Join(_context.Tests,
                a => a.Test_id,
                b => b.Id,
                (a, b) => new
                {
                    Id = a.Id,
                    Test_Name = b.Name,
                }).ToList();


            List<Dictionary<string, object>> testsList = new List<Dictionary<string, object>>();

            foreach (var test in tests)
            {
                testsList.Add(new Dictionary<string, object>() {
                    { "Id", test.Id },
                    { "Test_Name", test.Test_Name},
                });
            }
            ViewBag.Tests = testsList;
            //---------------------------------
            var studDocs = _context.StudDocuments.
                Where(a=>a.Student_id == id)
                .Join(_context.Documents,
                a => a.Document_id,
                b => b.Id,
                (a, b) => new
                {
                    Id = a.Id,
                    Document_id = a.Document_id,
                    Url = a.Url,
                    Doc_name = a.Doc_name,
                    created_at = a.created_at.ToShortDateString(),
                    Document_Teacher = b.Doc_name

                })
                .ToList();


            obj = new List<Dictionary<string, object>>();

            foreach (var value in studDocs)
            {
                obj.Add(new Dictionary<string, object>() {
                    { "Id", value.Id },
                    { "Document_id", value.Document_id },
                    { "Document_Teacher", value.Document_Teacher },
                    { "Url", value.Url },
                    { "Doc_name", value.Doc_name },
                    { "created_at", value.created_at }
                });
            }
            ViewBag.StudDocuments = obj;
            return View(usersList);
        }
        [HttpPost]
        public async Task<IActionResult> CreateGrade(int id,int grade,int user_id) {
            Grade Grade = new Grade {User_id=user_id, Grades = grade,Date=DateTime.Now};

            _context.Grades.Add(Grade);
            await _context.SaveChangesAsync();
            return RedirectToAction("DetailsGroup", new { id = id });
        }

        public IActionResult CreateTest(int id)
        {
            ViewBag.Group_id = id;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateTest(int id,string name, Dictionary<int,string> quest, Dictionary<int, List<string>> answer)
        {
            int max_id = 1;
            try
            {
                max_id = _context.Tests.Max(p => p.Id)+1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Tests test = new Tests {Id = max_id, Group_id = id, Name = name };
            Console.WriteLine(max_id+" "+id+" "+name);
            _context.Tests.Add(test);
            await _context.SaveChangesAsync();
            foreach (KeyValuePair<int, string> value in quest)
            {
                int max_id2 = 1;
                try
                {
                    max_id2 = _context.Questions.Max(p => p.Id)+1;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                Question question = new Question {Id = max_id2, Test_id = max_id, Question_title = value.Value };
                Console.WriteLine(max_id2 + " " + max_id + " " + value.Value);

                _context.Questions.Add(question);
                await _context.SaveChangesAsync();
                foreach (KeyValuePair<int, List<string>> value2 in answer)
                {
                    if (value2.Key == value.Key)
                        foreach (string value3 in value2.Value)
                        {
                            Answer answer1 = new Answer { Question_id = max_id2, Answer_text = value3 };
                            Console.WriteLine(max_id2 + " " + value3);
                            _context.Answers.Add(answer1);
                            await _context.SaveChangesAsync();
                            Console.WriteLine("value3 value " + value3);
                        }
                }
            }
            return RedirectToAction("ViewTests", new { id = id});
        }

        public IActionResult ViewTests(int id)
        {
            ViewBag.Group_id = id;
            var users = _context.Tests.Where(x=>x.Group_id == id);
            List<Dictionary<string, object>> usersList = new List<Dictionary<string, object>>();

            foreach (var user in users)
            {
                usersList.Add(new Dictionary<string, object>() {
                    { "Id", user.Id },
                    { "Name", user.Name }
                });
            }
            return View(usersList);
        }
        public async Task<IActionResult> DeleteTest(int  id,int group_id)
        {
            Tests users = _context.Tests.Find(id);
            Console.WriteLine(users.Id);
            _context.Tests.Remove(users);
            var q = _context.Questions.Where(x => x.Test_id == id);
            foreach (var question in q)
            {
                var a = _context.Answers.Where(x => x.Question_id == question.Id);
                foreach (var answer in a)
                {
                    Console.WriteLine(answer.Id);
                    _context.Answers.Remove(answer);
                }
                _context.Questions.Remove(question);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("ViewTests",new { id = group_id});
        }

        public IActionResult DetailsTest(int id)
        {
            ViewBag.Group_id = id;
            var tests = _context.Tests.Where(x => x.Id == id);
            var questions = _context.Questions.Where(x => x.Test_id == id);
            var answer = _context.Answers.Where(o => questions.Any(h => h.Id == o.Question_id));
            ViewBag.tests = tests.ToList();
            ViewBag.questions = questions.ToList();
            ViewBag.answers = answer.ToList();
            return View();
        }
        public IActionResult AddDocument(int id)
        {
            ViewBag.Group_id = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddDocument(IFormFile Url, int Group_id, string Name)
        {
            if (Url != null)
            {
                // путь к папке Files
                string path = "/Files/" + Url.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await Url.CopyToAsync(fileStream);
                }
                Document file = new Document { Name = Name, Url = path ,Group_id = Group_id, Doc_name = Url.FileName, created_at = DateTime.Now.Date };
                _context.Documents.Add(file);
                _context.SaveChanges();
            }

            return RedirectToAction("DetailsGroup",new { id = Group_id });
        }
        public IActionResult ViewDocuments(int id)
        {
            ViewBag.Group_id = id;
            return View(_context.Documents.Where(x => x.Group_id == id));
        }
        public IActionResult DeleteDocument(int id, int group_id)
        {
            ViewBag.Group_id = group_id;
            _context.Documents.Remove(_context.Documents.Find(id));
            _context.SaveChanges();
            return RedirectToAction("ViewDocuments", new { id = group_id });
        }
        public IActionResult DownloadDocument(int id)
        {
            string path = _context.Documents.Find(id).Url;
            string file_path = _appEnvironment.WebRootPath + path;
            string file_type = "application/octet-stream";
            string file_name = _context.Documents.Find(id).Doc_name;
            return PhysicalFile(file_path, file_type, file_name);
        }
        public IActionResult ViewTimeTable()
        {
            //ViewBag.Group_id = id;
            var users = _context.Groups
            .Where(a => a.Supervisor_id == int.Parse(User.Identity.Name))
            .Join(_context.Timetables,
            a => a.Id,
            b => b.Group_id,
            (a, b) => new
            {
                Id = a.Id,
                Name = a.Name,
                Week_day = b.Week_day,
                Time = b.Time,
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
                    { "Name", user.Name },
                    { "Week_day", week },
                    { "Time", user.Time },
                });
            }
            return View(usersList);
        }
        public IActionResult DownloadStudDocument(int id)
        {

            string path = _context.StudDocuments.Find(id).Url;
            string file_path = _appEnvironment.WebRootPath + path;
            string file_type = "application/octet-stream";
            string file_name = _context.StudDocuments.Find(id).Doc_name;
            return PhysicalFile(file_path, file_type, file_name);
        }
        public IActionResult ViewTestAnswers(int id)
        {
            ViewBag.Test_Name = _context.Users_Tests    
                .Where(a => a.Id == id)
                .Join(_context.Tests,
                a => a.Test_id,
                b => b.Id,
                (a, b) => new
                    {
                        Name = b.Name,
                    }
                )
                .ToList()[0].Name;

            var tests = _context.Users_Tests_Answers
            .Where(a => a.Users_Tests_Id == id)
            .Join(_context.Questions,
                a => a.Question_id,
                b => b.Id,
                (a, b) => new
                {
                    Question = b.Question_title,
                    Answer_id = a.Answer_id,
                }
                )
            .Join(_context.Answers,
                a => a.Answer_id,
                b => b.Id,
                (a, b) => new
                {
                    Question = a.Question,
                    Answer = b.Answer_text,
                }
                )
            .ToList();
            //Console.WriteLine(tests.Count);
            List<Dictionary<string, object>> testList = new List<Dictionary<string, object>>();

            foreach (var test in tests)
            {
                Console.WriteLine(1);
                testList.Add(new Dictionary<string, object>() {
                    { "Question", test.Question },
                    { "Answer", test.Answer },
                });
            }
            ViewBag.Answers = testList;
            return View();
            //return View(tests.Count.ToString());
        }




    }
}
