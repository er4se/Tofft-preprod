using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tofft_preprod.DbContext;
using Tofft_preprod.Models;

namespace Tofft_preprod.Controllers
{
    /// <summary>
    /// Контроллер администратора, позволяет</br>
    /// управлять участниками проекта
    /// </summary>
    [Authorize(Policy = "RequireUser")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Просмотр заявок на втупление в проект
        /// </summary>
        /// <param name="boardId"></param>
        /// <returns></returns>
        [Authorize(Policy = "BoardAdmin")]
        [HttpGet]
        public async Task<IActionResult> Requests(string id)
        {
            var requests = await _context.JoinRequests
                .Include(r => r.User)
                .Where(r => r.BoardId == id && r.Status == JoinStatus.Pending)
                .ToListAsync();

            return View(requests);
        }

        /// <summary>
        /// Одобрение заявки на вступление в проект
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Approve(string requestId)
        {
            var request = await _context.JoinRequests
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == requestId);

            if (request == null) return NotFound();

            request.Status = JoinStatus.Approved;

            var userToBoard = new UserToBoard
            {
                UserId = request.UserId,
                BoardId = request.BoardId,
                Role = BoardRole.Memeber
            };

            await _context.UserToBoards.AddAsync(userToBoard);
            await _context.SaveChangesAsync();

            return RedirectToAction("Requests", new { id = request.BoardId });
        }
    }
}
