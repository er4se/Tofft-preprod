using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Tofft_preprod.Models;
using Tofft_preprod.DbContext;
using Microsoft.AspNetCore.Identity;
using Tofft_preprod.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Tofft_preprod.Repositories;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Tofft_preprod.Controllers
{
    //public class BoardIdRequiredFilter : IActionFilter
    //{
    //    public void OnActionExecuting(ActionExecutingContext context)
    //    {
    //        var boardId = context.RouteData.Values["boardId"] as string;
    //        if (string.IsNullOrEmpty(boardId))
    //        {
    //            // Логика, если boardId не найден (например, редирект)
    //            context.Result = new RedirectResult("Index");
    //        }
    //    }
    //
    //    public void OnActionExecuted(ActionExecutedContext context)
    //    {
    //    }
    //}

    [Authorize(Policy = "RequireUser")]
    [Route("Board/{boardId}/Mission/[action]/{id?}")]
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

        [Authorize(Policy = "BoardMember")]
        [HttpGet]
        public async Task<IActionResult> Index(string boardId, string id)
        {
            var mission = await _missionRepository.GetByIdAsync(id);

            if (mission is null)
                return RedirectToAction("Error");

            var vm = new MissionIndexViewModel();
            vm.Mission = mission;

            return View(vm);
        }

        [Authorize(Policy = "BoardMember")]
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(string boardId, string id)
        {
            var entity = await _missionRepository.GetByIdAsync(id);
            var user = await _userManager.GetUserAsync(User);

            switch (entity.Status)
            {
                case MissionStatus.Available:
                    entity.Status = MissionStatus.Processing;
                    entity.ExecutorId = user.Id;
                    break;
                case MissionStatus.Processing:
                    entity.Status = MissionStatus.Done;
                    break;
                default:
                    Console.WriteLine("Не возможно изменить статус");
                    //Добавить Error по строке\\
                    break;
            }

            await _missionRepository.UpdateAsync(entity);
            return RedirectToAction("Index", new { boardId, id });
        }

        #region Edit

        [Authorize(Policy = "BoardLead")]
        [HttpGet]
        public async Task<IActionResult> Edit(string boardId, string id)
        {
            var mission = await _missionRepository.GetByIdAsync(id);
            var dto = new MissionDTO
            {
                Title = mission.Title,
                Description = mission.Description,
                Deadline = mission.Deadline,
                Group = mission.Group
            };

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string boardId, string id, MissionDTO dto)
        {
            var user = await _userManager.GetUserAsync(User);
            var mission = await _missionRepository.GetByIdAsync(id);

            if (ModelState.IsValid)
            {
                mission.Title = dto.Title;
                mission.Description = dto.Description;
                mission.Deadline = dto.Deadline;
                mission.Group = dto.Group;

                await _missionRepository.UpdateAsync(mission);

                return RedirectToAction("Index", new { boardId, id });
            }

            return RedirectToAction("Error");
        }

        #endregion

        #region Create

        [Authorize(Policy = "BoardLead")]
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

                return RedirectToAction("Index", new { boardId, mission.Id });
            }

            return RedirectToAction("Error");
        }

        #endregion

        [Authorize(Policy = "BoardLead")]
        [HttpPost]
        public async Task<IActionResult> Delete(string boardId, string id)
        {
            var entity = await _missionRepository.GetByIdAsync(id);
            await _missionRepository.DeleteAsync(entity);
            return RedirectToAction("Details", "Board", new { boardId });
        }
    }
}