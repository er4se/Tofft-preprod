using Microsoft.EntityFrameworkCore;
using Tofft_preprod.DbContext;
using Tofft_preprod.Models;

namespace Tofft_preprod.Services
{
    public interface IBoardAuthService
    {
        Task<bool> HasRoleAsync(string boardId, string userId, BoardRole requiredRole);
    }
    
    public class BoardAuthService : IBoardAuthService
    {
        private readonly ApplicationDbContext _context;

        public BoardAuthService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> HasRoleAsync(string boardId, string userId, BoardRole requiredRole)
        {
            var userBoard = await _context.UserToBoards
                .FirstOrDefaultAsync(ub =>
                    ub.BoardId == boardId &&
                    ub.UserId == userId);

            if (userBoard == null) return false;

            return userBoard.Role <= requiredRole;
        }
    }
}
