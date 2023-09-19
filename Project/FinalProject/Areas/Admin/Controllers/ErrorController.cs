using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ErrorController : Controller
	{
		public async Task<IActionResult> Index()
		{
			return View();
		}
	}
}
