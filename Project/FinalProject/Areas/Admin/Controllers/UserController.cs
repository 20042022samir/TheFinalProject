using FinalProject.Contexts;
using FinalProject.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly FinalProjectDatbase _context;
        private readonly UserManager<AppUser> _usermanagger;
        private readonly RoleManager<IdentityRole> _rolemanager;
        public UserController(FinalProjectDatbase context, UserManager<AppUser> usermanagger,RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _usermanagger = usermanagger;
            _rolemanager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var userRole = await _rolemanager.FindByNameAsync("User");

            if (userRole == null)
                return NotFound("The 'User' role does not exist.");
            
            var usersInUserRole = await _usermanagger.GetUsersInRoleAsync(userRole.Name);

            IEnumerable<AppUser> usersEnumerable = usersInUserRole;
            
            return View(usersEnumerable);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserHouses(string username)
        {
            AppUser? user = await _usermanagger.FindByNameAsync(username);
            if (user == null)
                return NotFound();
            GetUserHouseVModel model = new();
            model.user=user;
            model.Houses = await _context.Houses.Where(x => !x.IsDeleted).Include(x => x.houseImages.Where(x => !x.IsDeleted)).ToListAsync();
            return View(model);
        }

        
    }

}

    public class GetUserHouseVModel
    {
        public AppUser user { get; set; }
        public IEnumerable<House> Houses { get; set; }
    }