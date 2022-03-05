using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using aspnet_edu_center.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Web;

namespace aspnet_edu_center.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        [Authorize]
        public IActionResult Index()
        {
            if (User.IsInRole("1"))
            {
                return RedirectToAction("Index", "Admin");
            }
            else if (User.IsInRole("2"))
            {
                return RedirectToAction("Index", "Teacher");
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
