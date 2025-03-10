using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Tofft_preprod.Models;
using Tofft_preprod.DbContext;
using Microsoft.AspNetCore.Identity;
using Tofft_preprod.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Tofft_preprod.Controllers
{
    [Authorize(Policy = "RequireUser")]
    public class BoardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public BoardController(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpPost]
        public ActionResult ActionToIndex(string id)
        {
            return RedirectToAction("Index", new {id = id});
        }

        [HttpGet]
        public async Task<IActionResult> Index(string id)
        {
            var user = await _userManager.GetUserAsync(User);
            var viewModel = new BoardViewModel();

            var board = await _context.Boards
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            viewModel.Board = board;

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Taskboard()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult BoardCreation(){
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UserBoards()
        {
            var user = await _userManager.GetUserAsync(User);
            var viewModel = new BoardViewModel();

            //viewModel.UserBoards = new List<Board>();
            viewModel.UserBoards = await _context.Boards
                .Where(b => b.AdminId == user.Id)
                .ToListAsync();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind(Prefix = "Board")] Board board)
        {
            ModelState.Remove("Board.Id");
            ModelState.Remove("Board.AdminId");

            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                board.AdminId = user.Id;
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new { x.Key, x.Value.Errors });

                foreach (var error in errors)
                {
                    Console.WriteLine($"Key: {error.Key}, Errors: {string.Join(", ", error.Errors.Select(e => e.ErrorMessage))}");
                }
            }

            if (ModelState.IsValid)
            {
                await _context.Boards.AddAsync(board);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
