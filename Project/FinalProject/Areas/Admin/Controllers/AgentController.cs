using FinalProject.Contexts;
using FinalProject.Core.Entities;
using FinalProject.Extentions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AgentController : Controller
    {
        private readonly FinalProjectDatbase _context;
        private readonly IWebHostEnvironment _environment;
        public AgentController(FinalProjectDatbase context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Agent> agents = await _context
                .Agents.Where(x => !x.IsDeleted)
                 .ToListAsync();
            return View(agents);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Agent agent)
        {
            agent.Image = agent.file.CreateImage(_environment.WebRootPath, "Assets/Images");
           
            await _context.Agents.AddAsync(agent);
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
          Agent? agennt=  await _context.Agents.Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();
            if (agennt == null)
                return RedirectToAction("index", "error");
            return View(agennt);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, Agent agent)
        {
            Agent? agentUpdate = await _context.Agents
                .Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();
            agentUpdate.FullName=agent.FullName;
            agentUpdate.PhoneNumber=agent.PhoneNumber;
            agentUpdate.Email = agent.Email;
            if (agent.file != null)
            {
            agentUpdate.Image = agent.file.CreateImage(_environment.WebRootPath, "Assets/Images");  
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> DetailAgent(int id)
        {
            Agent? agent = await _context.Agents
                .Where(x => !x.IsDeleted && x.Id == id)
                .Include(x => x.agentedHouses.Where(x=>!x.IsDeleted))
                .FirstOrDefaultAsync();
            if (agent == null)
                return RedirectToAction("index", "error");
            return View(agent);
        }
    }


}
