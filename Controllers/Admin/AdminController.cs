using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using aspnet_edu_center.Models;
using aspnet_edu_center.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace aspnet_edu_center.Controllers.Admin
{
    [Authorize(Roles = "1")]
    public class AdminController : Controller
    {
        private ApplicationContext _context;
        public AdminController(ApplicationContext context)
        {
            _context = context;
        }
        
        //Groups-------------------------------------------------------------------------
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ViewUsers()
        {
            //DbSet<User> users = _context.Users;
            var users = _context.Users
            .Join(_context.Roles,
                a => a.Role_id,
                b => b.Id,
                (a, b) => new
                {
                    Id = a.Id,
                    Name = a.Name,
                    Email = a.Email,
                    Password = a.Password,
                    Tel_num = a.Tel_num,
                    Role = b.Name,
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
                    { "Password", user.Password },
                    { "Tel_num", user.Tel_num },
                    { "Role", user.Role }
                });
            }
            return View(usersList);
        }
        public IActionResult CreateUser()
        {
            List<Dictionary<string, object>> RolesList = new List<Dictionary<string, object>>();
            foreach (var role in _context.Roles.ToList())
            {
                RolesList.Add(new Dictionary<string, object>() {
                    { "Id", role.Id },
                    { "Name", role.Name },
                });
            }
            ViewBag.Roles = RolesList;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user == null)
                {
                    // добавляем пользователя в бд
                    user = new User { Email = model.Email, Password = model.Password, Name = model.Name, Tel_num = model.Tel_num, Role_id = model.Role_id };

                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("ViewUsers");
                }
                else
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }

        public IActionResult EditUser(int id)
        {
            List<Dictionary<string, object>> RolesList = new List<Dictionary<string, object>>();
            foreach (var role in _context.Roles.ToList())
            {
                RolesList.Add(new Dictionary<string, object>() {
                    { "Id", role.Id },
                    { "Name", role.Name },
                });
            }
            ViewBag.Roles = RolesList;
            User users = _context.Users.FirstOrDefault(u => u.Id == id);
            return View(users);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(User model)
        {
            //if (ModelState.IsValid)
            //{
                User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == model.Id);
                if (user != null)
                {
                    user.Email = model.Email;
                    user.Name = model.Name;
                    user.Tel_num = model.Tel_num;
                    user.Role_id = model.Role_id;
                    user.Password = model.Password;
                    _context.Entry(user).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
            //ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            //}
            return RedirectToAction("ViewUsers");
        }

        public IActionResult DetailsUser(int id)
        {

            User user = _context.Users.FirstOrDefault(u => u.Id == id);
            return View(user);
        }
        public IActionResult DeleteUser(int id)
        {

            User user = _context.Users.FirstOrDefault(u => u.Id == id);
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser(User model)
        {
            _context.Users.Remove(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("ViewUsers");
        }

        //Groups-------------------------------------------------------------------------

        public IActionResult ViewGroups()
        {
            //DbSet<Group> groups = _context.Groups;
            var groups = _context.Groups
            .Join(_context.Group_types,
            a => a.Group_type,
            b => b.Id,
            (a, b) => new
            {
                Id = a.Id,
                Name = a.Name,
                Supervisor_id = a.Supervisor_id,
                Group_type = b.Name,
                date_form = a.date_form.ToShortDateString(),
                date_to = a.date_to.ToShortDateString(),
            }
            )
            .Join(_context.Users,
            a => a.Supervisor_id,
            b => b.Id,
            (a, b) => new
            {
                Id = a.Id,
                Name = a.Name,
                Supervisor = b.Name,
                Group_type = a.Group_type,
                date_form = a.date_form,
                date_to = a.date_to,
            }
            )
            .ToList();
            List<Dictionary<string, object>> groupsList = new List<Dictionary<string, object>>();
            foreach (var group in groups)
            {
                groupsList.Add(new Dictionary<string, object>() {
                    { "Id", group.Id },
                    { "Name", group.Name },
                    { "Supervisor", group.Supervisor },
                    { "Group_type", group.Group_type },
                    { "date_form", group.date_form },
                    { "date_to", group.date_to }
                });
            }
            return View(groupsList);
        }
        public IActionResult CreateGroup()
        {
            List<Dictionary<string, object>> Group_typesList = new List<Dictionary<string, object>>();
            foreach (var Group_type in _context.Group_types.ToList())
            {
                Group_typesList.Add(new Dictionary<string, object>() {
                    { "Id", Group_type.Id },
                    { "Name", Group_type.Name },
                });
            }
            ViewBag.Group_types = Group_typesList;
            List<Dictionary<string, object>> UsersList = new List<Dictionary<string, object>>();
            foreach (var user in _context.Users.Where(u => u.Id == 2).ToList())
            {
                UsersList.Add(new Dictionary<string, object>() {
                    { "Id", user.Id },
                    { "Name", user.Name },
                });
            }
            ViewBag.Users = UsersList;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateGroup(Group model)
        {
            if (ModelState.IsValid)
            {
                Group group = await _context.Groups.FirstOrDefaultAsync(u => u.Name == model.Name);
                if (group == null)
                {
                    group = new Group
                    {
                        Name = model.Name,
                        Group_type = model.Group_type,
                        Supervisor_id = model.Supervisor_id,
                        date_form = model.date_form,
                        date_to = model.date_to
                    };
                    _context.Groups.Add(group);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("ViewGroups");
                }
                else
                    ModelState.AddModelError("Ошибка", "Некорректные данные!");
            }
            return View(model);
        }

        public IActionResult EditGroup(int id)
        {
            List<Dictionary<string, object>> Group_typesList = new List<Dictionary<string, object>>();
            foreach (var Group_type in _context.Group_types.ToList())
            {
                Group_typesList.Add(new Dictionary<string, object>() {
                    { "Id", Group_type.Id },
                    { "Name", Group_type.Name },
                });
            }
            ViewBag.Group_types = Group_typesList;
            List<Dictionary<string, object>> UsersList = new List<Dictionary<string, object>>();
            foreach (var user in _context.Users.Where(u => u.Id == 2).ToList())
            {
                UsersList.Add(new Dictionary<string, object>() {
                    { "Id", user.Id },
                    { "Name", user.Name },
                });
            }
            ViewBag.Users = UsersList;
            Group groups = _context.Groups.FirstOrDefault(u => u.Id == id);
            return View(groups);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditGroup(Group model)
        {
            Group group = await _context.Groups.FirstOrDefaultAsync(u => u.Id == model.Id);
            if (group != null)
            {
                group.Name = model.Name;
                group.Group_type = model.Group_type;
                group.Supervisor_id = model.Supervisor_id;
                group.date_form = model.date_form;
                group.date_to = model.date_to;
                _context.Entry(group).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("ViewGroups");
        }

        public IActionResult DetailsGroup(int id)
        {

            Group group = _context.Groups.FirstOrDefault(u => u.Id == id);
            return View(group);
        }
        public IActionResult DeleteGroup(int id)
        {

            Group group = _context.Groups.FirstOrDefault(u => u.Id == id);
            return View(group);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteGroup(Group model)
        {
            _context.Groups.Remove(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("ViewGroups");
        }
        

        public IActionResult ViewGroup_Users(int id)
        {
            //DbSet<User> users = _context.Users.Where(u => u.Id == model.Id);
            ViewBag.id = id;
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
                )
                .Where(a=>a.Group_Id==id).ToList();
            List<Dictionary<string, object>> usersList = new List<Dictionary<string, object>>();
            foreach (var user in users)
            {
                usersList.Add(new Dictionary<string, object>() {
                    { "Id", user.Id },
                    { "Name", user.Name },
                    { "Email", user.Email }, 
                    { "Tel_num", user.Tel_num }, 
                    { "Group_Id", user.Group_Id } 
                });
            }
                //Where(a => a.Group_Id=id);
            return View(usersList);
        }
        public IActionResult AddUser(int id, int group_id)
        {
            if (group_id != 0)
            {
                _context.Group_Users.Add(new Group_User { Group_Id = group_id, User_Id = id });
                _context.SaveChanges();
                return RedirectToAction("ViewGroup_Users", new { id = group_id });
            }
            ViewBag.Id = id;
            var users = from Users in _context.Users
                        join Group_Users in _context.Group_Users on Users.Id equals Group_Users.User_Id into ps
                        from Group_Users in ps.DefaultIfEmpty()
                        where Group_Users.Group_Id == null
                        select new
                        {
                            Id = Users.Id,
                            Name = Users.Name,
                            Email = Users.Email,
                            Tel_num = Users.Tel_num,
                            User_Id = Group_Users.User_Id,
                        };
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

        public IActionResult DeleteGroup_User(int id, int group_id)
        {
            _context.Group_Users.Remove(_context.Group_Users.FirstOrDefault(u => u.User_Id == id));
            _context.SaveChanges();
            return RedirectToAction("ViewGroup_Users", new { id = group_id });
        }

        //Group_types-------------------------------------------------------------------------

        public IActionResult ViewGroup_types()
        {
            DbSet<Group_type> group_types = _context.Group_types;
            return View(group_types);
        }
        public IActionResult CreateGroup_type()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateGroup_type(Group_type model)
        {
            if (ModelState.IsValid)
            {
                Group_type group_type = await _context.Group_types.FirstOrDefaultAsync(u => u.Name == model.Name);
                if (group_type == null)
                {
                    group_type = new Group_type
                    {
                        Name = model.Name
                    };
                    _context.Group_types.Add(group_type);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("ViewGroup_types");
                }
                else
                    ModelState.AddModelError("Ошибка", "Некорректные данные!");
            }
            return View(model);
        }

        public IActionResult EditGroup_type(int id)
        {

            Group_type group_types = _context.Group_types.FirstOrDefault(u => u.Id == id);
            return View(group_types);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditGroup_type(Group_type model)
        {
            Group_type group_type = await _context.Group_types.FirstOrDefaultAsync(u => u.Id == model.Id);
            if (group_type != null)
            {
                group_type.Name = model.Name;
                _context.Entry(group_type).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("ViewGroup_types");
        }

        public IActionResult DetailsGroup_type(int id)
        {

            Group_type group_type = _context.Group_types.FirstOrDefault(u => u.Id == id);
            return View(group_type);
        }
        public IActionResult DeleteGroup_type(int id)
        {

            Group_type group_type = _context.Group_types.FirstOrDefault(u => u.Id == id);
            return View(group_type);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteGroup_type(Group_type model)
        {
            _context.Group_types.Remove(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("ViewGroup_types");
        }
        public IActionResult TimeTableGroup(int id)
        {
            ViewBag.Group_id = id;
            ViewBag.Group_Name = _context.Groups.FirstOrDefault(u => u.Id==id).Name;
            var timetable = _context.Timetables
                .Where(u => u.Group_id == id);
            List<Dictionary<string, object>> timetableList = new List<Dictionary<string, object>>();
            foreach (var user in timetable)
            {
                string week = "";
                switch (user.Week_day){
                    case 1:week = "Дүйсенбі"; break;
                    case 2:week = "Сейсенбі"; break;
                    case 3:week = "Сәрсенбі"; break;
                    case 4:week = "Бейсенбі"; break;
                    case 5:week = "Жұма"; break;
                    case 6:week = "Сенбі"; break;
                    case 7:week = "Жексенбі"; break;
                }
                timetableList.Add(new Dictionary<string, object>() {
                    { "Id", user.Id },
                    { "Week_day", week },
                    { "Time", user.Time }
                });
            }
            return View(timetableList);
        }
        public IActionResult CreateTimeTableGroup(int id)
        {
            ViewBag.Group_id = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTimeTableGroup(Timetable model)
        {
            Timetable timetable = new Timetable { Group_id = model.Group_id, Week_day = model.Week_day, Time = model.Time};
            _context.Timetables.Add(timetable);
            await _context.SaveChangesAsync();
            return RedirectToAction("TimeTableGroup", new {id = model.Group_id });
        }
        public IActionResult EditTimeTableGroup(int id)
        {
            ViewBag.Group_id = id;
            Timetable timetable = _context.Timetables.FirstOrDefault(u => u.Id == id);
            return View(timetable);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTimeTableGroup(Timetable model)
        {
            Timetable timetable = await _context.Timetables.FirstOrDefaultAsync(u => u.Id == model.Id);
            if (timetable != null)
            {
                timetable.Week_day = model.Week_day;
                timetable.Time = model.Time;
                
                _context.Entry(timetable).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("TimeTableGroup", new { id = model.Group_id });
        }
        public IActionResult DeleteTimeTableGroup(int id,int group_id)
        {
            _context.Timetables.Remove(_context.Timetables.FirstOrDefault(u => u.Id == id));
            _context.SaveChanges();
            return RedirectToAction("TimeTableGroup", new { id = group_id });
        }

    }
}
