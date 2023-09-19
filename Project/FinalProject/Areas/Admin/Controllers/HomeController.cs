using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin,SuperAdmin")]
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
           
            return View();
        }
    }
}
