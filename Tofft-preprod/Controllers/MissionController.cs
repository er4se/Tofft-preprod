using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Tofft_preprod.Models;
using Tofft_preprod.DbContext;
using Microsoft.AspNetCore.Identity;
using Tofft_preprod.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Tofft_preprod.Repositories;

namespace Tofft_preprod.Controllers
{
    [Authorize(Policy = "RequireUser")]
    [Route("Board/{boardId}/Mission/[action]")]
    public class MissionController: Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IRepository<Mission> _missionRepository;
        private readonly UserManager<User> _userManager;

        public MissionController(UserManager<User> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;

            _missionRepository = new MissionRepository(context);
        }

        [Authorize(Policy = "BoardLead")]
        [HttpGet]
        public IActionResult Index()
        {
            var mission = _context.Missions
                .Where(m => m.Status == MissionStatus.Available)
                .FirstOrDefault();

            if (mission is null)
                return RedirectToAction("Error");

            var vm = new MissionIndexViewModel();
            vm.Mission = mission;

            return View(vm);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(MissionDTO dto) //boardId падает
        {
            var user = await _userManager.GetUserAsync(User);
            var boardId = HttpContext.Request.RouteValues["boardId"] as string;
            
            if (ModelState.IsValid)
            {
                var mission = new Mission(dto);
                mission.CreatorId = user.Id;
                mission.BoardId = boardId;

                await _missionRepository.CreateAsync(mission);

                return RedirectToAction("Index");
            }

            return RedirectToAction("Error");
        }
    }
}