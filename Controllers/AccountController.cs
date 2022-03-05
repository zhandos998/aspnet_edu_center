using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using aspnet_edu_center.ViewModels;     // пространство имен моделей RegisterModel и LoginModel
using aspnet_edu_center.Models;         // пространство имен UserContext и класса User
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNet.Identity;


namespace aspnet_edu_center.Controllers
{

    namespace RolesApp.Controllers
    {
        public class AccountController : Controller
        {

            public const string DefaultIdClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
            private ApplicationContext _context;
            public AccountController(ApplicationContext context)
            {
                _context = context;
            }
            [HttpGet]
            public IActionResult Register()
            {
                return View();
            }
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Register(RegisterModel model)
            {
                if (ModelState.IsValid)
                {
                    User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                    if (user == null)
                    {
                        // добавляем пользователя в бд
                        user = new User { Email = model.Email, Password = model.Password };
                        Role userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "student");
                        if (userRole != null)
                            user.Role_id = userRole.Id;

                        _context.Users.Add(user);
                        await _context.SaveChangesAsync();

                        await Authenticate(user); // аутентификация

                        return RedirectToAction("Index", "Home");
                    }
                    else
                        ModelState.AddModelError("", "Некорректные логин и(или) пароль");
                }
                return View(model);
            }
            [HttpGet]
            public IActionResult Login()
            {
                return View();
            }
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Login(LoginModel model)
            {
                if (ModelState.IsValid)
                {
                    User user = await _context.Users
                        //.Include(u => u.Role_id)
                        .FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
                    if (user != null)
                    {
                        await Authenticate(user); // аутентификация

                        return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
                }
                return View(model);
            }
            private async Task Authenticate(User user)
            {
                // создаем один claim
                var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Id.ToString()),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role_id.ToString())
            };

                // создаем объект ClaimsIdentity
                ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);

                // установка аутентификационных куки
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
            }

            /// <summary>
            /// Send an OpenID Connect sign-out request.
            /// </summary>
            public async void SignOut()
            {
                await HttpContext.SignOutAsync();
                Response.Redirect("/");
            }
        }
    }
}