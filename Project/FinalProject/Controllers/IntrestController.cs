using FinalProject.Contexts;
using FinalProject.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace FinalProject.Controllers
{
    public class IntrestController : Controller
    {
        private readonly FinalProjectDatbase _context;
        private readonly UserManager<AppUser> _userManager;
        public IntrestController(FinalProjectDatbase context,UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager=userManager;
        }
        [HttpGet]
        public async Task<IActionResult> CreateIntrest()
        {
            if (User.Identity.IsAuthenticated == false)
            {
                TempData["Registered"] = "Mesaj ala bilmek ucun daxil olmalisiniz!";
                return RedirectToAction("index", "home");
            }

            string username = User.Identity.Name;
            AppUser? user=await _userManager.FindByNameAsync(username);
            ViewBag.User=user;
            ViewBag.Cities = await _context.Cities.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.Countries = await _context.Countries.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.Typees = await _context.Typees.Where(x => !x.IsDeleted).ToListAsync();
            if (User.Identity.IsAuthenticated == false)
            {
                TempData["Registered"] = "Mesaj almaq ucun daxil olun";
                return RedirectToAction("index", "home");
            }
            return View();
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateIntrest(IntrestMessage message)
        {
            ViewBag.Cities = await _context.Cities.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.Countries = await _context.Countries.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.Typees = await _context.Typees.Where(x => !x.IsDeleted).ToListAsync();
            string username = User.Identity.Name;
            AppUser? user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return NotFound();
            message.user = user;
            await _context.IntrestMessages.AddAsync(message);
            await _context.SaveChangesAsync();
            TempData["Registered"] = "Ugurla elave edildi :)";
            return RedirectToAction("index", "home");
        }
       

        
    }
   
}
