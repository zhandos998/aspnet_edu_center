using Microsoft.AspNetCore.Mvc;

namespace aspnet_edu_center.Controllers.Admin
{
    public class AdminGroupController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
