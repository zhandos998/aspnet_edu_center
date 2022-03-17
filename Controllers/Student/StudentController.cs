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

    }
}
