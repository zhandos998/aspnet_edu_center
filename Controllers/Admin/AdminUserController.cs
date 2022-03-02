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
    public class AdminUserController : Controller
    {
        private ApplicationContext _context;
        public AdminUserController(ApplicationContext context)
        {
            _context = context;
        }
        public IActionResult ViewGroups()
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
        [HttpGet]
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
        [HttpGet]
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
    }
}
