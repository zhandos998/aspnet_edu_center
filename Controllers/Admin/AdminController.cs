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
    public class AdminController : Controller
    {
        private ApplicationContext _context;
        public AdminController(ApplicationContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "1")]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ViewUsers()
        {
            DbSet<User> users = _context.Users;
            return View(users);
        }
        public IActionResult CreateUser()
        {
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

        //-------------------------------------------------------------------------

        public IActionResult ViewGroups()
        {
            DbSet<Group> groups = _context.Groups;
            return View(groups);
        }
        public IActionResult CreateGroup()
        {
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

        //-------------------------------------------------------------------------

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


    }
}
