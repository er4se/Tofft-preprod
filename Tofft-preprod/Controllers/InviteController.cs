using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tofft_preprod.DbContext;
using Tofft_preprod.Models;

namespace Tofft_preprod.Controllers
{
    //[Authorize]                                               WARNING
    public class InviteController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public InviteController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        //[Authorize(Policy = "BoardAdmin")]                    WARNING
        public async Task<IActionResult> GenerateLink(string boardId)
        {
            boardId = "d5c3f87b-ffd3-42a8-ad41-092286e7d919"; //WARNING 
            var token = Guid.NewGuid().ToString("N");
            var invite = new InviteLink
            {
                BoardId = boardId,
                Token = token,
                ExpirationDate = DateTime.UtcNow.AddDays(7),
                IsActive = true
            };

            await _context.InviteLinks.AddAsync(invite);
            await _context.SaveChangesAsync();

            var inviteUrl = Url.Action("AcceptInvite", "Invite",
                new { token }, Request.Scheme);

            //return NotFound();
            return Ok(new { Link = inviteUrl }); 
        }

        [HttpGet]
        public async Task<IActionResult> AcceptInvite(string token)
        {
            var invite = await _context.InviteLinks
                .FirstOrDefaultAsync(i =>
                    i.Token == token &&
                    i.IsActive &&
                    i.ExpirationDate > DateTime.UtcNow);

            if (invite == null) return View("InvalidInvite");

            var userId = _userManager.GetUserId(User);
            if (userId == null) return Challenge();

            var existingRequest = await _context.JoinRequests
                .FirstOrDefaultAsync(r =>
                    r.UserId == userId &&
                    r.BoardId == invite.BoardId);

            if (existingRequest != null)
                return View("RequestExists");

            var request = new JoinRequest
            {
                UserId = userId,
                BoardId = invite.BoardId,
                Status = JoinStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            await _context.JoinRequests.AddAsync(request);
            await _context.SaveChangesAsync();

            return View("RequestSubmitted");
        }
    }
}
