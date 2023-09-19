using FinalProject.Contexts;
using FinalProject.Core.Entities;
using FinalProject.Extentions;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net.Mail;
using System.Net;

namespace FinalProject.Controllers
{
    public class ElanController : Controller
    {
        
        private readonly UserManager<AppUser> _userManager;
        private readonly FinalProjectDatbase _context;
        private readonly IWebHostEnvironment _enviroment;

        public ElanController(UserManager<AppUser> userManager, FinalProjectDatbase context, IWebHostEnvironment enviroment)
        {
            _userManager = userManager;
            _context = context;
            _enviroment = enviroment;
        }
        [HttpGet]
        public async Task<IActionResult> ShareHouse()
        {
            if(User.IsInRole("SuperAdmin") || User.IsInRole("Admin"))
            {
                TempData["Registered"] = "Admin ve Superadminler elan vere bilmez!";
                return RedirectToAction("index", "home",new { showError =true});
            }
            if (User.Identity.IsAuthenticated == false)
            {
                TempData["Registered"] = "elan vermek ucun daxil olmalisiniz!";
                return RedirectToAction("index", "home", new { showError = true });
            }
            ViewBag.Types = await _context.Typees.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.Cities = await _context.Cities.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.Metros = await _context.Motros.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.Countries = await _context.Countries.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.Distincts = await _context.Distincts.Where(x => !x.IsDeleted).ToListAsync();
            return View();
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ShareHouse(House house)
        {
            ViewBag.Types = await _context.Typees.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.Cities = await _context.Cities.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.Metros = await _context.Motros.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.Countries = await _context.Countries.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.Distincts = await _context.Distincts.Where(x => !x.IsDeleted).ToListAsync();
            if (house.files == null)
            {
                TempData["Registered"] = "En Azindan 1 sekil elave etmelisiniz :)";
                return RedirectToAction("index", "home");
            }
            int counter = 0;
            foreach (var item in house.files)
            {
                await _context.HouseImages.AddAsync(new HouseImage()
                {
                    Image=item.CreateImage(_enviroment.WebRootPath,"Assets/Images"),
                    House=house,
                    IsMain = counter == 0 ? true : false,
                });
                counter++;
            }
            string username = User.Identity.Name;
            AppUser user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return NotFound();
            house.AppUser= user;
            house.CreatedDate = DateTime.Now;
            house.CreatedDate = DateTime.Now;
            await _context.Houses.AddAsync(house);
            await _context.SaveChangesAsync();
            var queryIntrest = _context.IntrestMessages.Where(x => !x.IsDeleted);
            if (house.CityId != null)
            {
                queryIntrest=queryIntrest.Where(x=>x.CityId==house.CityId);
            }
            if(house.TypeeId!=null)
            {
                queryIntrest = queryIntrest.Where(x => x.TypeId == house.TypeeId);
            }
            if (house.CountryId != null)
            {
                queryIntrest = queryIntrest.Where(x => x.CountryId == house.CountryId);
            }
            
            List<IntrestMessage>? intrests = await queryIntrest.Include(x=>x.user)
                .ToListAsync();
            if(intrests.Count() != 0)
            {
                foreach (var item in intrests)
                {
				string token = await _userManager.GenerateEmailConfirmationTokenAsync(item.user);
				var link = Url.Action(action: "detail", controller: "house", values: new { token = token, id = house.Id }, protocol: Request.Scheme);
				using (MailMessage mm = new MailMessage())
				{
					mm.From = new MailAddress("samir.ismayilov2004@gmail.com");
					mm.Subject = "Size maraqli olan yeni bir elan paylasilib :)";
					mm.To.Add(item.user.Email);
					mm.Body = $"<div style='display:flex;justify-content:center;align-items:center;background-color:grey;border-radius:10px;height:150px;' ><a href='{link}' style='text-decoration:none;padding:5px 7px;margin:auto;border:3px solid red;border-radius:10px; background-color:white;color:black;' >Eve baxin!</a></div>";
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
                }
			}
            TempData["Registered"] = "Ev Ugurla Paylasildi :)";
            return RedirectToAction("index", "home");
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            House? housse = await _context.Houses.Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();
            if(housse==null)
                return NotFound();
            housse.IsDeleted = true;
            await _context.SaveChangesAsync();
            TempData["Deleted"] = "Evi Sildiniz :)";
            return RedirectToAction("detailuser", "account");
        }

        [HttpPost]
        public async Task<IActionResult> ShareComment(DetailHouseViewModel model,int houseId)
        {
            if (User.Identity.IsAuthenticated == false)
            {
                TempData["Registered"] = "Comment Yazmaq ucun qeydiyyaztdan kecmelisiniz :)";
                return RedirectToAction("login", "account");
            }
            
            string username = User.Identity.Name;
            AppUser user=await _userManager.FindByNameAsync(username);
            if(user==null)
                return NotFound();
            Comment comment=new()
            {
                Description=model.Comment.Description,
                Tittle=user.Name,
                CreatedDate=DateTime.Now,
                HouseId=houseId,
                LikeCount=0,
                DislikCount=0,
                AppUser=user,
            };
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction("detail", "house", new { Id = houseId });
        }

        [HttpPost]
        public async Task<IActionResult> ShareReplyComment(DetailHouseViewModel model,int commentId,int houseId)
        {
            if (User.Identity.IsAuthenticated == false)
            {
                TempData["loggin"] = "Comment Yazmaq ucun qeydiyyaztdan kecmelisiniz :)";
                return RedirectToAction("login", "account");
            }
            string username= User.Identity.Name;
            AppUser user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return NotFound();
            ReplyComment comment = new()
            {
                Description = model.ReplyComment.Description,
                CreatedDate = DateTime.Now,
                CommentId = commentId,
                LikeCount = 0,
                DislikeCount = 0,
                AppUser = user
            };
            await _context.replyComments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction("detail", "house", new { Id = houseId });
        }
        [HttpGet]
        public async Task<IActionResult> DeleteComment(int id)
        {
           Comment? comment= await _context.Comments.Where(x => !x.IsDeleted && x.Id == id)
                .Include(x=>x.House)
                .FirstOrDefaultAsync();
            if (comment == null)
                return NotFound();
            comment.IsDeleted= true;
            await _context.SaveChangesAsync();
            return RedirectToAction("detail", "house", new { Id = comment.House.Id });
        }
        [HttpGet]
        public async Task<IActionResult> DeleteReplyComment(int id)
        {
            ReplyComment? comment = await _context.replyComments
                .Where(x => !x.IsDeleted && x.Id == id).Include(x=>x.Comment)
                .ThenInclude(x=>x.House)
                .FirstOrDefaultAsync();
            if (comment == null)
                return NotFound();
            comment.IsDeleted= true;
            await _context.SaveChangesAsync();
            return RedirectToAction("detail", "house", new { Id = comment.Comment.House.Id });
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> IncreaseLike(int id,int houseId)
        {
            Comment? comment=await _context.Comments
                .Where(x=>!x.IsDeleted && x.Id==id)
                .FirstOrDefaultAsync();
            if(comment==null)
                return NotFound();
            comment.LikeCount++;
            await _context.SaveChangesAsync();
            return RedirectToAction("detail", "house", new { Id = houseId});
        }

    }

}


