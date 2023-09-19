using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
	public class ErrorController : Controller
	{

		public async Task<IActionResult> Index()
		{
			return View();
		}
	}
}
