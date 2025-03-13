using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tofft_preprod.DbContext;
using Tofft_preprod.Repositories;

namespace Tofft_preprod.Models.ViewModels
{
    public class MissionController : Controller
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

        [HttpPost]
        public async Task<IActionResult> Create(MissionDTO dto)
        {
            var user = await _userManager.GetUserAsync(User);
            
            if (ModelState.IsValid)
            {
                Mission mission = new Mission(dto);
                mission.CreatorId = user.Id;

                await _missionRepository.CreateAsync(mission);
                return RedirectToAction("Index");
            }

            return RedirectToAction("Error");
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(string id)
        {
            var mission = await _missionRepository.GetByIdAsync(id);
            mission.Status++;

            await _missionRepository.UpdateAsync(mission);

            return RedirectToAction("Index");
        }
    }
}
