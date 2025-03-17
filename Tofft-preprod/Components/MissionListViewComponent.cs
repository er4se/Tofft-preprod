using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tofft_preprod.DbContext;
using Tofft_preprod.Models;

namespace Tofft_preprod.Components
{
    public class MissionListViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public MissionListViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string boardId, MissionStatus status)
        {
            var missions = await _context.Missions
                .Where(m => m.BoardId == boardId && m.Status == status)
                .ToListAsync();

            return View(missions);
        }
    }
}
