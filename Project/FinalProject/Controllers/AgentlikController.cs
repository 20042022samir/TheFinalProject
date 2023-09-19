using FinalProject.Contexts;
using FinalProject.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
	public class AgentlikController : Controller
	{
		private readonly FinalProjectDatbase _context;
		private readonly UserManager<AppUser> _userManager;
		public AgentlikController(FinalProjectDatbase context,UserManager<AppUser> userManager)
		{
			_context=context;
			_userManager=userManager;
		}
		public async Task<IActionResult> Index()
		{

            IEnumerable<Agent> agents = await _context.Agents
				.Where(x => !x.IsDeleted)
				.ToListAsync();
			return View(agents);
		}

		[HttpGet]
		public async Task<IActionResult> Detail(int id)
		{

            Agent? agent = await _context.Agents
				.Where(x => !x.IsDeleted && x.Id == id)
				.Include(x => x.agentedHouses.Where(x=>!x.IsDeleted))
				.ThenInclude(x => x.Images)
				.FirstOrDefaultAsync();
			if (agent == null)
				return NotFound();
			return View(agent);
		}

		[HttpGet]
		public async Task<IActionResult> DetailAgeentedHouse(int id)
		{

            AgentedHouse? house = await _context.AgentedHouses
				.Where(x => !x.IsDeleted && x.Id == id)
				.Include(x => x.Images)
				.Include(x=>x.Agent)
				.FirstOrDefaultAsync();
			if(house==null)
				return NotFound();
			return View(house);
		}

		[HttpGet]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(int id)
		{

            Agent? agent = await _context.Agents
			.Where(x => !x.IsDeleted && x.Id == id)
			.FirstOrDefaultAsync();
			if (agent == null)
				return RedirectToAction("index", "home");
			return View(agent);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(int id,Agent agent)
		{

            if (!ModelState.IsValid)
				return View();
            Agent? agentUpdate = await _context.Agents
            .Where(x => !x.IsDeleted && x.Id == id)
            .FirstOrDefaultAsync();
            if (agent == null)
                return RedirectToAction("index", "home");
			agentUpdate.FullName = agent.FullName;
			agentUpdate.PhoneNumber = agent.PhoneNumber;
			agentUpdate.Email=agent.Email;
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
        }
	}
}
