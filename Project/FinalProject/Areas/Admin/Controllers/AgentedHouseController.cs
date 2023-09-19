using FinalProject.Contexts;
using FinalProject.Core.Entities;
using FinalProject.Extentions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AgentedHouseController : Controller
    {
        private readonly FinalProjectDatbase _context;
        private readonly IWebHostEnvironment _enviroment;
        public AgentedHouseController(FinalProjectDatbase context, IWebHostEnvironment enviroment)
        {
            _context = context;
            _enviroment = enviroment    ;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<AgentedHouse> houses = await _context
           .AgentedHouses.Where(x => !x.IsDeleted)
           .Include(x => x.Images)
            .Include(x => x.Agent)
            .ToListAsync();
            return View(houses);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Agents = await _context.Agents
                .Where(x => !x.IsDeleted)
                .ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AgentedHouse house)
        {
            if (house.files.Count() == 0)
            {
                ModelState.AddModelError(string.Empty, "at least one image must be entered!");
                return View(house);
            }
            int counter = 0;
            foreach (var item in house.files)
            {
                AgentedHousePhoto photoo = new()
                {       
                    AgentedHouse = house,
                    Image = item.CreateImage(_enviroment.WebRootPath, "Assets/Images"),
                    IsMain=counter==0?true:false,
                    CreatedDate=DateTime.Now
                };
                await _context.agentedHousePhotos.AddAsync(photoo);
                counter++;
            }
            house.CreatedDate = DateTime.Now;
            await _context.AgentedHouses.AddAsync(house);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            AgentedHouse? house = await _context.AgentedHouses
                .Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();
            if (house == null)
                return RedirectToAction("index", "house");
            house.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
