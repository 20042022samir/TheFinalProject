using FinalProject.Core.Entities;
using FinalProject.Extentions;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using MimeKit;
using FinalProject.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _enviroment;
        private readonly FinalProjectDatbase _context;
        private readonly MailSender _mailSender;
        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInmanager,RoleManager<IdentityRole> roleManager,IWebHostEnvironment enviroment,FinalProjectDatbase context,MailSender mailSender)
        {
               _userManager= userManager;
            _signInManager= signInmanager;
            _roleManager= roleManager;
            _enviroment= enviroment;
            _context= context;
            _mailSender= mailSender;
        }
        [HttpGet]
        public async Task<IActionResult> CreateRole()
        {
            IdentityRole role1 = new() { Name = Roles.SuperAdmin.ToString() };
            IdentityRole role2 = new() { Name = Roles.Admin.ToString() };
            IdentityRole role3 = new() { Name = Roles.User.ToString() };
            await _roleManager.CreateAsync(role1);
            await _roleManager.CreateAsync(role2);
            await _roleManager.CreateAsync(role3);
            return Json("CreatedRoles!");
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Emphty = true;
                return View(model);
            }
            ViewBag.Emphty = false;
            AppUser? user=await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "name or password incorrect!");
                return View(model);
            }
            var resultt= await _signInManager.PasswordSignInAsync(user, model.Password, model.IsRememberMe, true);
            if (!resultt.Succeeded)
            {
                if (resultt.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "you have been blocked for 1 minute!");
                    return View(model);     
                }
                ModelState.AddModelError(string.Empty, "name or password incorrect! 1");
                return View(model);
            }           
                var userRoles = await _userManager.GetRolesAsync(user);
                if (userRoles.Contains("Admin") || userRoles.Contains("SuperAdmin"))
                    TempData["Registered"] = "Siz adminlerden birisiniz ;)";          
            return RedirectToAction("index", "home");
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            
            AppUser user = new AppUser()
            {
                Name = model.Name,
                UserName = model.UserName,
                Surname = model.Surname, 
                Email = model.Email,
                Image = model.file != null ? model.file.CreateImage(_enviroment.WebRootPath, "Assets/Images"):""
            };
           var result= await _userManager.CreateAsync(user,model.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
                return View(model);
            }
            await _userManager.AddToRoleAsync(user, "User");
			string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
			var link = Url.Action(action: "VerifyEmail", controller: "account", values: new { token = token, email = user.Email }, protocol: Request.Scheme);
			using (MailMessage mm = new MailMessage())
			{
				mm.From = new MailAddress("samir.ismayilov2004@gmail.com");
				mm.Subject = "Verify Email";
				mm.To.Add(user.Email);
				mm.Body = $"<div style='display:flex;justify-content:center;align-items:center;background-color:grey;border-radius:10px;height:150px;' ><a href='{link}' style='text-decoration:none;padding:5px 7px;margin:auto;border:3px solid red;border-radius:10px; background-color:white;color:black;' >mailnizzi tesdiqleyin!</a></div>";
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
			TempData["Registered"] = "Zehmet olmasa mailinizi yoxlayin :)";
			return RedirectToAction("index", "home");
		}

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }

        [HttpGet]
        public async Task<IActionResult> DetailUser()
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
                return RedirectToAction("index", "error");
            DetailUserViewModel model = new();
            model.user=user;
            model.houses = await _context.Houses.Where(x => x.AppUser == user && !x.IsDeleted).Include(x => x.houseImages)
                .Include(x=>x.City)
                .Include(x=>x.Country)
                .Include(x=>x.Metro)
                .ToListAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AdminCreate()
        {
            AppUser SuperAdmin = new()
            {
                Name = "SuperAdmin",
                Surname = "Adminov",
                Email = "Admin@gmail.com",
                UserName = "Killer12"
            };
            await _userManager.CreateAsync(SuperAdmin, "Salam123@");
            AppUser Admin = new()
            {
                Name = "Admin",
                Surname = "Adminovski",
                Email = "Admin@mail.ru",
                UserName = "Killer31"
            };
            await _userManager.CreateAsync(Admin, "Admin12345@");
            await _userManager.AddToRoleAsync(SuperAdmin, "SuperAdmin");
            await _userManager.AddToRoleAsync(Admin, "Admin");
            return Json("Okay");
        }

 
        [HttpGet]
        [Authorize(Roles ="SuperAdmin")]
        public async Task<IActionResult> CreateAdmin()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAdmin(CreateAdminViewModel model)
        {
            if(!ModelState.IsValid)
                return View(model);
            AppUser user = new()
            {
                Name=model.Name,
                UserName=model.UserName,
                Email=model.Email,
                Surname=model.Surname,
                Image = model.file != null ? model.file.CreateImage(_enviroment.WebRootPath, "Assets/Images") : ""
            };
            var result= await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
                return View(model);
            }
            await _userManager.AddToRoleAsync(user, "Admin");
            TempData["Registered"] = "new admin succesfuully created!";
            return RedirectToAction("index", "home");
        }

        [HttpGet]
        [Authorize(Roles="SuperAdmin")]
        public async Task<IActionResult> ShowAllAdmins()
        {
            var adminRole = await _roleManager.FindByNameAsync("Admin");
            if (adminRole == null)
                return NotFound();
            IEnumerable<AppUser> Admins = await _userManager.GetUsersInRoleAsync(adminRole.Name);
            return View(Admins);
        }

        [HttpGet]
		public async Task<IActionResult> VerifyEmail(string token, string email)
		{
			var user = await _userManager.FindByEmailAsync(email);
			if (user == null)
				return NotFound();
			await _userManager.ConfirmEmailAsync(user, token);
			await _signInManager.SignInAsync(user, true);
			return RedirectToAction("index", "home");
		}

        [HttpGet]
        public async Task<IActionResult> Update()
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
                return NotFound();
            UserUpdateViewModel model = new()
            {
                UserName= user.UserName,
                Email=user.Email,
                Name=user.Name,
                Surname=user.Surname
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UserUpdateViewModel model)
        {
            
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            if(user==null)
                return NotFound();
            user.Name = model.Name; user.Email=model.Email; user.Surname = model.Surname;user.UserName=model.UserName;
            user.Image = model.file != null ? model.file.CreateImage(_enviroment.WebRootPath, "Assets/Images") : user.Image;


			var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
                return View(model);
            }
            if (!string.IsNullOrWhiteSpace(model.NewPassword))
            {
                var resulltPassword = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (!resulltPassword.Succeeded)
                {
                    foreach (var item in resulltPassword.Errors)
                    {
                        ModelState.AddModelError(string.Empty, item.Description);
                    }
                    return View(model);
                }
            }
            await _signInManager.SignInAsync(user, true);
            TempData["Registered"] = "Ugurla Guncellendi :)";
            return RedirectToAction("index", "home");
        }

        [HttpGet]
        public async Task<IActionResult> ForgetPassword()
        {
            return View();
        }

		[HttpPost]
		public async Task<IActionResult> ForgetPassword(string email)
		{
			var user = await _userManager.FindByEmailAsync(email);
			if (user == null)
            {
                TempData["Registered"] = "Bele bir emaile sahib istifadeci tapilmadi :(";
                return RedirectToAction("index", "home");
            }
			string token = await _userManager.GeneratePasswordResetTokenAsync(user);

			var link = Url.Action(action: "resetpassword", controller: "account", values: new { token = token, email = email }, protocol: Request.Scheme);


			MimeMessage emailMessage = new MimeMessage();
			//MailboxAddress emailFrom = new MailboxAddress("Samir Ismayilov", _emailSettings.EmailId);
			using (MailMessage mm = new MailMessage())
			{
				mm.From = new MailAddress("samir.ismayilov2004@gmail.com");
				mm.Subject = "reset password";
				mm.To.Add(user.Email);
				mm.Body = $"<a href='{link}'>passwordu deyisin :)</a> ";
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
			TempData["Registered"] = "Zehmet olmasa emailinizi yoxlayin :)";
			return RedirectToAction("index", "home");
		}

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound();

            ResetPasswordViewModel model = new()
            {

            };
            return View(model);
        }

        [HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user == null)
				return NotFound();
			var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

			if (!result.Succeeded)
			{
				List<object> errors = new List<object>();
				foreach (var item in result.Errors)
				{
					errors.Add(item.Description);
				}
				return Json(errors);
			}
            TempData["Registered"] = "Passwordunuz ugurla deyisildi";
			return RedirectToAction("login", "account");

		}

        [HttpPost]
        public async Task<IActionResult> SerachUser(string username)
        {
            if (username == null)
            {
                TempData["Registered"] = "axtarmaq ucun istifadeci adi daxil edin!";
                return RedirectToAction("index", "home");
            }
            AppUser? user = await _userManager.FindByNameAsync(username);
            if(user==null)
            {
                TempData["Registered"] = "Bele bir istifadeci yoxdur :)";
                return RedirectToAction("index", "home");
            }
			var userRoles = await _userManager.GetRolesAsync(user);

			if (userRoles.Contains("Admin") || userRoles.Contains("SuperAdmin"))
			{
                TempData["Registered"] = "bele bir istifadeci yoxdur";
                return RedirectToAction("index", "home");
			}
			ViewBag.Houses = await _context.Houses.Where(x => !x.IsDeleted && x.AppUser.Name == user.Name).ToListAsync();
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> GetSearchedUserHOuses(string username)
        {
            AppUser? user=await _userManager.FindByNameAsync(username);
            if (user == null)
                return NotFound();
            IEnumerable<House> Houses = await _context.Houses
                 .Where(x => !x.IsDeleted && x.AppUser == user)
                 .Include(x => x.houseImages.Where(x => !x.IsDeleted))
                 .Include(x=>x.Comments.Where(x=>!x.IsDeleted))
                 .ToListAsync();
            ViewBag.User=user;
            return View(Houses);
            
        }

        [HttpGet]
        [Authorize(Roles="SuperAdmin")]
        public async Task<IActionResult> DeleteAdmin(string adminName)
        {
           AppUser? user=  await _userManager.FindByNameAsync(adminName);
            if(user==null)
                return NotFound();
           var result=  await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                TempData["Registered"] = "Silinmedi";
                return RedirectToAction("index", "home");
            }
            TempData["Registered"] = "Admin ugurla silindi";
            return RedirectToAction("index", "home");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteProfilPhoto(string username)
        {
            AppUser? user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return NotFound();
            user.Image = "";
            await _context.SaveChangesAsync();
            TempData["Registered"] = "Profil sekli ugurla silindi";
            return RedirectToAction("index", "home");
        }

       
      


    }

    public class REsponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
   

}

//Samir123
//Samir123@


//Messi123
//Messi123@

//Ryan123
//Ryan123@

//Eminem123
//Eminem123@

//Bear123
//Bear123@