using FinalProject.Contexts;
using FinalProject.Core.Entities;
using FinalProject.Extentions;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using System.Drawing.Printing;
using System.Linq.Expressions;

namespace FinalProject.Controllers
{
    public class HouseController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly FinalProjectDatbase _context;
        private readonly IWebHostEnvironment _enviroment;
        private readonly SignInManager<AppUser> _signInManager;

        public HouseController(UserManager<AppUser> userManager,FinalProjectDatbase context, IWebHostEnvironment enviroment, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _context = context;
            _enviroment = enviroment;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Detail(int id)
        {
         House? house=   await _context.Houses.Where(x => !x.IsDeleted && x.Id == id)
                .Include(x => x.houseImages.Where(x=>!x.IsDeleted))
                .Include(x => x.City).Include(x => x.Distinct).Include(x => x.Country)
                .Include(x => x.Metro).Include(x => x.Typee).Include(x=>x.AppUser)
                .Include(x=>x.Comments.Where(x=>!x.IsDeleted))
                .ThenInclude(x=>x.AppUser)
                .Include(x=>x.Comments.Where(x=>!x.IsDeleted))
                .ThenInclude(x=>x.ReplyComments.Where(x=>!x.IsDeleted))
                .ThenInclude(x=>x.AppUser)
                .FirstOrDefaultAsync();
            if(house==null)
                return NotFound();
            house.ViewCount++;
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                ViewBag.UserImage = user.Image;
            }
            else
            {
                ViewBag.UserImage = "";
            }
            DetailHouseViewModel model = new()
            {
                house=house,
               Comments=house.Comments
            };
            
            await _context.SaveChangesAsync();
            if (house.Typee != null)
            {
                model.ExtraHouses = await _context.Houses
                .OrderByDescending(x=>x.CreatedDate)
                .Where(x => !x.IsDeleted && x.Id != id && x.Typee==house.Typee)
                .Include(x => x.houseImages)
                .Include(x => x.Distinct).Include(x => x.Country)
                .Include(x => x.City).Take(6)
                .ToListAsync();
            }
            else
            {
                model.ExtraHouses = await _context.Houses
                .OrderByDescending(x => x.CreatedDate)
                .Where(x => !x.IsDeleted && x.Id != id)
                .Include(x => x.houseImages)
                .Include(x => x.Distinct).Include(x => x.Country)
                .Include(x => x.City).Take(6)
                .ToListAsync();
            }
            return View(model);
        }

        [HttpGet]
        
        public async Task<IActionResult> CountryDetail(int id,int page = 1, int pageSize = 5)
        {
            Country? country = await _context.Countries
                .Where(x => !x.IsDeleted && x.Id == id)
                .Include(x => x.Houses.Where(x=>!x.IsDeleted).OrderByDescending(x => x.CreatedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize))
                .ThenInclude(x=>x.houseImages)
                .FirstOrDefaultAsync();
            if (country == null)
                return NotFound();
            ViewBag.Name=country.Name;
            ViewBag.CountryId=country.Id;
            var totalItems = await _context.Houses.Where(x => !x.IsDeleted && x.CountryId==id).CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var viewModel = new PagginationViewModel<House>
            {
                Items = country.Houses,
                PageNumber = page,
                PageSize = pageSize,
                TotalPages = totalPages
            };
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetRentedHouses(int page = 1, int pageSize = 5)
        {
            List<House> Houses = await _context
                .Houses.Where(x => !x.IsDeleted && x.ForRent).Include(x => x.houseImages)
                .Include(x => x.Distinct).Include(x => x.City)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.OrderByDescending(x=>x.CreatedDate)
                .ToListAsync();
			var totalItems = await _context.Houses.Where(x => !x.IsDeleted && x.ForRent).CountAsync();
			var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
			var viewModel = new PagginationViewModel<House>
			{
				Items = Houses,
				PageNumber = page,
				PageSize = pageSize,
				TotalPages = totalPages
			};
			return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetSaledHouses(int page = 1, int pageSize = 5)
        {
            List<House> Houses = await _context
                .Houses.Where(x => !x.IsDeleted && x.ForSale).Include(x => x.houseImages)
                .Include(x => x.Distinct).Include(x => x.City)
                .Include(x=>x.Comments)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .OrderByDescending(blog => blog.CreatedDate)
                .ToListAsync();
			var totalItems = await _context.Houses.Where(x=>!x.IsDeleted && x.ForSale).CountAsync();
			var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
			var viewModel = new PagginationViewModel<House>
			{
				Items = Houses,
				PageNumber = page,
				PageSize = pageSize,
				TotalPages = totalPages
			};
			return View(viewModel);
        }
        [HttpGet]
        public async Task<IActionResult> CityDetail(int id, int page = 1, int pageSize = 5)
        {
            City? city = await _context.Cities
         .Where(c => !c.IsDeleted && c.Id == id)
          .Include(c => c.Houses.Where(h => !h.IsDeleted).OrderByDescending(x => x.CreatedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize))
           .ThenInclude(x=>x.houseImages)
            .FirstOrDefaultAsync();      
            if (city == null)
                return NotFound();
            ViewBag.Name = city.Name;
            ViewBag.Id = city.Id;
            var totalItems = await _context.Houses.Where(x => !x.IsDeleted && x.CityId == id).CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var viewModel = new PagginationViewModel<House>
            {
                Items = city.Houses,
                PageNumber = page,
                PageSize = pageSize,
                TotalPages = totalPages
            };
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> TypeeDetail(int id, int page = 1, int pageSize = 5)
        {
            Typee? typee = await _context.Typees.Where(x => !x.IsDeleted && x.Id == id)
                .Include(x => x.Houses.Where(x=>!x.IsDeleted).OrderByDescending(x => x.CreatedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)).ThenInclude(x=>x.houseImages)
                .FirstOrDefaultAsync();
            
            if (typee == null)
                return NotFound();
            ViewBag.Name = typee.Name;
            ViewBag.Id = typee.Id;
            var totalItems = await _context.Houses.Where(x => !x.IsDeleted && x.TypeeId == id).CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var viewModel = new PagginationViewModel<House>
            {
                Items = typee.Houses,
                PageNumber = page,
                PageSize = pageSize,
                TotalPages = totalPages
            };
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllHouses(int page=1,int pageSize=6)
        {
           List<House> Houses= await _context.Houses.Where(x => !x.IsDeleted)
				.OrderByDescending(blog => blog.CreatedDate)
                .Include(x => x.houseImages.Where(x=>!x.IsDeleted))
                .Include(x => x.City).Include(x => x.Distinct)
                .Include(x=>x.Comments.Where(x=>!x.IsDeleted))
                .ThenInclude(x=>x.ReplyComments.Where(x=>!x.IsDeleted))
                .Include(x => x.Metro).Include(x => x.Country)
                .Include(x => x.AppUser)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();
			var totalItems = await _context.Houses.Where(x=>!x.IsDeleted).CountAsync();
			var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
			var viewModel = new PagginationViewModel<House>
			{
				Items = Houses,
				PageNumber = page,
				PageSize = pageSize,
				TotalPages = totalPages
			};
			return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetMostVatchedHouses(int page = 1, int pageSize = 6)
        {
			List<House> Houses = await _context.Houses.Where(x => !x.IsDeleted)
				.OrderByDescending(blog => blog.ViewCount)
				.Include(x => x.houseImages)
				.Include(x => x.City).Include(x => x.Distinct)
				.Include(x => x.Comments.Where(x => !x.IsDeleted))
				.Include(x => x.Metro).Include(x => x.Country)
				.Include(x => x.AppUser)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();
			var totalItems = await _context.Houses.CountAsync();
			var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

			var viewModel = new PagginationViewModel<House>
			{
				Items = Houses,
				PageNumber = page,
				PageSize = pageSize,
				TotalPages = totalPages
			};
			return View(viewModel);
		}


        [HttpGet]
        public async Task<IActionResult> UpdateHouse(int id)
        {
            ViewBag.Cities = await _context.Cities.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.Distincts = await _context.Distincts.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.Metros = await _context.Motros.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.Countries = await _context.Countries.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.Types = await _context.Typees.Where(x => !x.IsDeleted).ToListAsync();
           House? house= await _context.Houses.Where(x => !x.IsDeleted && x.Id == id)
                .Include(x => x.houseImages).FirstOrDefaultAsync();
            if (house == null)
                return NotFound();
            return View(house);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateHouse(int id,House house)
        {
            ViewBag.Cities = await _context.Cities.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.Distincts = await _context.Distincts.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.Metros = await _context.Motros.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.Countries = await _context.Countries.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.Types = await _context.Typees.Where(x => !x.IsDeleted).ToListAsync();
            House? houseUpdate=  await _context.Houses.Where(x => !x.IsDeleted && x.Id == id)
                .Include(x=>x.houseImages)
                .FirstOrDefaultAsync();
            if (house.files!= null)
            {
                foreach (var item in houseUpdate.houseImages)
                {
                    item.IsDeleted = true;
                }
                int a = 0;
                foreach (var item in house.files)
                {
                    HouseImage image = new()
                    {
                        Image = item.CreateImage(_enviroment.WebRootPath, "Assets/Images"),
                        HouseId = id,
						IsMain = a == 0 ? true : false,
					};
                    a++;
                    await _context.HouseImages.AddAsync(image);
                }
            }
            else
            {
                houseUpdate.houseImages = houseUpdate.houseImages;
            }
            houseUpdate.Description = house.Description; houseUpdate.UpdatedDate = DateTime.Now;
            houseUpdate.FullName = house.FullName; houseUpdate.Price = house.Price;houseUpdate.RoomCount=house.RoomCount;
            houseUpdate.Size=house.Size;houseUpdate.Adress=house.Adress; houseUpdate.CityId=house.CityId;houseUpdate.RoomCount = house.RoomCount;
            houseUpdate.DistinctId = house.DistinctId;houseUpdate.MetroId = house.MetroId;houseUpdate.CountryId = house.CountryId;houseUpdate.TypeeId = house.TypeeId;
            if(houseUpdate.ForSale && house.ForRent)
            {
                houseUpdate.ForSale = false;
                houseUpdate.ForRent = true;
            }
            if(houseUpdate.ForRent && house.ForSale)
            {
                houseUpdate.ForSale = true;
                houseUpdate.ForRent = false;
            }
       
            await _context.SaveChangesAsync();
            TempData["Registered"] = "Eviniz Ugurla Guncellendi :)";
            return RedirectToAction("index", "home");
        }


        [HttpGet]
        public async Task<IActionResult> AddToFavorite(int houseId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["Registered"] = "Secilmislere elave etmek ucun daxil olmalisiniz!";
                return RedirectToAction("index", "home");
            }
          House?  house=  await _context.Houses.Where(x => !x.IsDeleted && x.Id == houseId)
                .Include(x => x.houseImages.Where(x => !x.IsDeleted))
                .FirstOrDefaultAsync();
            if (house == null)
                return NotFound();
            string username = User.Identity.Name;
            AppUser? user=await _userManager.FindByNameAsync(username);
            if (user == null)
                return NotFound();
            IEnumerable<Favorite> favorites = await _context.Favorities
                .Where(x => !x.IsDeleted && x.AppUser == user).Include(x=>x.House)
                .ToListAsync();
            foreach (var item in favorites)
            {
                if (item.House.FullName.Trim().ToLower() == house.FullName.Trim().ToLower())
                {

                    TempData["Registered"] = "Bunu zaten elave etmisiniz :)";
                    return RedirectToAction("index", "home");
                }
            }
            Favorite favorite = new()
            {
                AppUser= user,
                HouseId=houseId,
            };
            if (user.Image != "")
                favorite.Image = house.houseImages.FirstOrDefault(x => x.IsMain).Image;
            await _context.Favorities.AddAsync(favorite);
            await _context.SaveChangesAsync();
            TempData["Registered"] = "Elave edildi";
            return RedirectToAction("index", "home");
        }

        [HttpGet]
        public async Task<IActionResult> ShowFavorites()
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["Registered"] = "Secilmislere baxmaaq ve elave etmek ucun daxil olmalisiniz!";
                return RedirectToAction("index", "home");
            }
            string username=User.Identity.Name;
            AppUser? user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return NotFound();
            IEnumerable<Favorite> favorites = await _context.Favorities
                .Where(x => !x.IsDeleted && x.AppUser == user)
                .Include(x=>x.House).ThenInclude(x=>x.houseImages.Where(x=>!x.IsDeleted))
                .ToListAsync();
            return View(favorites);
        }

        [HttpGet]
        public async Task<IActionResult> RemovefromFavorites(int id)
        {
            Favorite? favorite = await _context.Favorities
                .Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();
            if (favorite == null)
                return NotFound();
            favorite.IsDeleted = true;
            await _context.SaveChangesAsync();
            TempData["Removed"] = "Ugurla secilmislerden silindi :)";
            return RedirectToAction("showfavorites", "house");
        }


        [HttpGet]
        [Authorize(Roles="Admin,SuperAdmin")]
        public async Task<IActionResult> DeleteByAdminShow(int id)
        {
          House? house=  await _context.Houses.Where(x => !x.IsDeleted && x.Id == id)
                .Include(x => x.AppUser).FirstOrDefaultAsync();
            if (house == null)
                return NotFound();
            return View(house);
        }
        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]

        public async Task<IActionResult> DeleteByAdmin(int id,string description)
        {
             House? house=  await _context.Houses.Where(x => !x.IsDeleted && x.Id == id)
                .Include(x => x.AppUser).FirstOrDefaultAsync();
            if (house == null)
                return NotFound();
            house.IsDeleted = true;
            Message message = new()
            {
                ForDelete = true,
                ForAnswer=false,
                AppUser = house.AppUser,
                Description = description,
                CreatedDate=DateTime.Now
            };
            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();
            TempData["Registered"] = "Admin kimi ugurla sildiniz :)";
            return RedirectToAction("index", "home");
        }

        [HttpGet]
        public async Task<IActionResult> ShowMessages()
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["Registered"] = "Mesajlari ala bilmek ucun daxil olun :)";
                return RedirectToAction("index", "home");
            }
            string username = User.Identity.Name;
            AppUser? user=await _userManager.FindByNameAsync(username);
            if (user == null)
                return NotFound();
            IEnumerable<Message> messages = await _context.Messages
                .Where(x => !x.IsDeleted && x.AppUser == user ).ToListAsync();
            return View(messages);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteMessage(int messageId)
        {
           Message? message= await _context.Messages.Where(x => !x.IsDeleted && x.Id == messageId).FirstOrDefaultAsync();
            if (message == null)
                return NotFound();
            message.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ShowMessages));
        }

        [HttpGet]
        public async Task<IActionResult> OpenComments(int id)
        {
            House? house = await _context.Houses.Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();
            if (house == null)
                return NotFound();
            house.IsCommentsOpen = false;
            await _context.SaveChangesAsync();
            TempData["Registered"] = "Evin Commentleri ugurla aktiv edildi";
            return RedirectToAction("index", "home");
        }

        [HttpGet]
        public async Task<IActionResult> CloseComments(int id)
        {
            House? house = await _context.Houses.Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();
            if (house == null)
                return NotFound();
            house.IsCommentsOpen = true;
            await _context.SaveChangesAsync();
            TempData["Registered"] = "Evin Commentleri ugurla deaktiv edildi";
            return RedirectToAction("index", "home");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> MakePremium(int houseId)
        {
            House? house = await _context.Houses.Where(x => !x.IsDeleted && x.Id == houseId)
                .FirstOrDefaultAsync();
            if(house==null)
                return NotFound();
            if (house.IsPremium == true)
            {
                TempData["Registered"] = "Bu elan zaten Premiumdur";
                return RedirectToAction("index", "home");
            }
            house.IsPremium = true;
            await _context.SaveChangesAsync();
            TempData["Registered"] = "Elan Ugurla Premium Edildi";
            return RedirectToAction("index", "home");
        }

      
        
    }

}
