using FinalProject.Contexts;
using FinalProject.Core.Entities;
using FinalProject.Models;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;

namespace FinalProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly FinalProjectDatbase _context;
        private readonly UserManager<AppUser> _userManager;
        public HomeController(FinalProjectDatbase context,UserManager<AppUser> manager)
        {
            _context = context;
            _userManager = manager;
        }

        public async Task<IActionResult> Error()
        {
            ViewBag.StatusCode = 404; // Set the status code for the view
            return View("_Error");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
           
            HomeViewModel model = new();
            model.houses = await _context.Houses
                .Where(x => !x.IsDeleted)
                .Include(x => x.houseImages.Where(x=>!x.IsDeleted))
                .Include(x => x.City)
                .Include(x => x.Distinct)
                .OrderByDescending(x=>x.CreatedDate)
                .Take(12)
                .ToListAsync();
            model.Prices = await _context.Prices
                .Where(x => !x.IsDeleted)
                .ToListAsync();
            model.Advertises = await _context.Advertises
                .Where(x => !x.IsDeleted)
                .ToListAsync();
            model.SalePrices = await _context.SalePrices
                .Where(x => !x.IsDeleted).ToListAsync();
            model.Typees = await _context.Typees
                .Where(x => !x.IsDeleted)
                .ToListAsync();
            model.Cities = await _context.Cities
                .Where(x => !x.IsDeleted).ToListAsync();
            model.Metros = await _context.Motros.Where(x => !x.IsDeleted)
                .ToListAsync();
            model.Countries = await _context.Countries
                .Where(x => !x.IsDeleted).ToListAsync();
            model.Distincts = await _context.Distincts
                .Where(x => !x.IsDeleted).ToListAsync();
            if (User.Identity.IsAuthenticated)
            {
                string username = User.Identity.Name;
               AppUser? user=await _userManager.FindByNameAsync(username);
                ViewBag.UsersFavorites = await _context.Favorities
                    .Where(x => !x.IsDeleted && x.AppUser==user).Include(x => x.House).Include(x => x.AppUser)
                    .ToListAsync();
            }
            return View(model);
        }

        public async Task<IActionResult> AboutPage()
        {

            IEnumerable<AboutPage> pages = await _context
                .aboutPages.Where(x => !x.IsDeleted)
                .ToListAsync();
            return View(pages);
        }

        [HttpPost]
		public async Task<IActionResult> FilterRentedHouses(HomeViewModel model)
		{
           if(model.SalePriceId==null && model.CityId==null && model.SalePriceId == null && model.TypeeId==null && model.RoomCount==null && model.MetroId==null && model.CountryId==null ) 
            {
                TempData["Registered"] = "Brini secmelisiniz :)";
                return RedirectToAction("index", "home");
            }
			var query = _context.Houses
				.Where(x => !x.IsDeleted && x.ForSale);
            if (model.SalePriceId != null)
            {
				SalePrice? pricee = await _context.SalePrices
			   .Where(x => !x.IsDeleted && x.Id == model.SalePriceId).FirstOrDefaultAsync();
				query = query.Where(x=>x.Price <= pricee.ToPrice && x.Price >= pricee.FromPrice);
			}

            if (model.CountryId.HasValue)
            {
				query = query.Where(x => x.CountryId == model.CountryId);
			}
            if (model.MetroId.HasValue)
            {
                query=query.Where(x=>x.MetroId==model.MetroId);
            }

			if (model.CityId.HasValue)
			{
				query = query.Where(x => x.CityId == model.CityId);
			}

			if (model.TypeeId.HasValue)
			{
				query = query.Where(x => x.TypeeId == model.TypeeId);
			}
			
			if (model.RoomCount.HasValue)
			{
				query = query.Where(x => x.RoomCount == model.RoomCount);
			}

			var filteredHouses = await query.Include(x=>x.houseImages)
								.Include(x => x.Comments.Where(x => !x.IsDeleted))
                                .Include(x=>x.AppUser)
				.ToListAsync();
            return View(filteredHouses);
		}

        [HttpPost]
        public async Task<IActionResult> GetSealedHouses(HomeViewModel model)
        {
			if (model.PriceId == null && model.CityId == null 
                && model.PriceId == null && model.TypeeId == null 
                && model.RoomCount == null && model.MetroId==null
                && model.DistinceId==null && model.CountryId==null
                && model.CityId==null
                )
			{
				TempData["Registered"] = "Brini secmelisiniz :)";
				return RedirectToAction("index", "home");
			}
			var query = _context.Houses
				.Where(x => !x.IsDeleted && x.ForRent);
			if (model.PriceId != null)
			{
				Price? pricee = await _context.Prices
			   .Where(x => !x.IsDeleted && x.Id == model.PriceId).FirstOrDefaultAsync();
				query = query.Where(x => x.Price <= pricee.ToPrice && x.Price >= pricee.FromPrice );
			}

			if (model.CountryId.HasValue)
			{
				query = query.Where(x => x.CountryId == model.CountryId);
			}
			if (model.MetroId.HasValue)
			{
				query = query.Where(x => x.MetroId == model.MetroId);
			}

			if (model.CityId.HasValue)
			{
				query = query.Where(x => x.CityId == model.CityId);
			}

			if (model.TypeeId.HasValue)
			{
				query = query.Where(x => x.TypeeId == model.TypeeId);
			}
            if (model.DistinceId.HasValue)
            {
                query = query.Where(x => x.DistinctId == model.DistinceId);
            }


			if (model.RoomCount.HasValue)
			{
				query = query.Where(x => x.RoomCount == model.RoomCount);
			}

			var filteredHouses = await query.Include(x => x.houseImages)
								.Include(x => x.Comments.Where(x => !x.IsDeleted))
				.ToListAsync();
			return View(filteredHouses);
		}

        [HttpGet]
        public async Task<IActionResult> Muraciet()
        {
			if (User.IsInRole("Admin") || User.IsInRole("SuperAdmin"))
			{
				TempData["Registered"] = "Adminler muraciet ede bilmez!";
				return RedirectToAction("index", "home");
			}
			return View();
        }

        [HttpPost]
        
        public async Task<IActionResult> Muraciet(Muraciet muraciet)
        {
            if (User.Identity.IsAuthenticated)
            {
                string username = User.Identity.Name;
                AppUser? user = await _userManager.FindByNameAsync(username);
                if (user == null)
                    return NotFound();
                muraciet.AppUser = user;
                muraciet.CreatedDate = DateTime.Now;
                await _context.Muraciets.AddAsync(muraciet);
                await _context.SaveChangesAsync();
				TempData["Registered"] = "Muraciet Edildi :)";
				return RedirectToAction("index", "home");
			}
            muraciet.CreatedDate = DateTime.Now;
            await _context.Muraciets.AddAsync(muraciet);
            await _context.SaveChangesAsync();
            TempData["Registered"] = "Muraciet Edildi :)";
            return RedirectToAction("index", "home");
        }

        [HttpGet]
        [Authorize(Roles ="Admin,SuperAdmin")]
        public async Task<IActionResult> ShowMuraciets()
        {
            IEnumerable<Muraciet> Muraciets = await _context
                .Muraciets.Where(x => !x.IsDeleted)
                .OrderByDescending(x=>x.CreatedDate)
                .Include(x => x.AppUser)
                .ToListAsync();
            return View(Muraciets);
        }

        [HttpGet]
        [Authorize(Roles="Admin,SuperAdmin")]
        public async Task<IActionResult> DeleteMuraciet(int id)
        {
           Muraciet? muraciet= await _context.Muraciets.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (muraciet == null)
                return NotFound();
            muraciet.IsDeleted = true;
            await _context.SaveChangesAsync();
            TempData["Registered"] = "Muraciet Silindi!";
            return RedirectToAction("index", "home");
        }

        [HttpGet]
        public async Task<IActionResult> ResponseMuraciet(int id)
        {
            Muraciet? muraciet = await _context.Muraciets
                .Where(x => !x.IsDeleted && x.Id == id)
                .Include(x=>x.AppUser)
                .FirstOrDefaultAsync();
            if (muraciet == null)
                return NotFound();
            return View(muraciet);
        }
        [HttpPost]
        public async Task<IActionResult> ResponseMuraciet(int id,string message)
        {
            Muraciet? muraciet = await _context.Muraciets
                .Where(x => !x.IsDeleted && x.Id == id)
                .Include(x => x.AppUser)
                .FirstOrDefaultAsync();
            if (muraciet == null)
                return NotFound();
            Message messageToSend = new()
            {
                ForAnswer=true,
                ForDelete=false,
                AppUser=muraciet.AppUser,
                Description=message,
                CreatedDate=DateTime.Now
            };
            string token = await _userManager.GenerateEmailConfirmationTokenAsync(muraciet.AppUser);
            var link = Url.Action(action: "showmessages", controller: "house", values: new { token = token, email = muraciet.AppUser.Email }, protocol: Request.Scheme);
            using (MailMessage mm = new MailMessage())
            {
                mm.From = new MailAddress("samir.ismayilov2004@gmail.com");
                mm.Subject = "Yeni Mesajlariniz Var";
                mm.To.Add(muraciet.AppUser.Email);
                mm.Body = $"<div style='display:flex;justify-content:center;align-items:center;background-color:grey;border-radius:10px;height:150px;' ><a href='{link}' style='text-decoration:none;padding:5px 7px;margin:auto;border:3px solid red;border-radius:10px; background-color:white;color:black;' >Mesajlarinizzi Gorun tesdiqleyin!</a></div>";
                mm.IsBodyHtml = true;
                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential("samirfi@code.edu.az", "ffhmkxihkkvuxeod");
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    try
                    {
                        smtp.Send(mm);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            muraciet.IsAnswered= true;
            await _context.Messages.AddAsync(messageToSend);
            await _context.SaveChangesAsync();
            TempData["Registered"] = "Ugurla cavab yazildi!";
            return RedirectToAction("index", "home");
        }

            
    }
}