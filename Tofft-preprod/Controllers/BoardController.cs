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
    public class BoardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IRepository<Board> _repository;
        private readonly UserManager<User> _userManager;

        public BoardController(UserManager<User> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;

            _repository = new BoardRepository(context);
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

            var board = await _repository.GetByIdAsync(id);

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
            viewModel.UserBoards = await _context.Boards    //UserToBoard actually
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
                await _repository.CreateAsync(board);

                var userBoard = new UserToBoard
                {
                    UserId = user.Id,
                    BoardId = board.Id,
                    UserLocalSpeciality = "Administrator" // Указываем должность
                };
                
                await _context.UserToBoards.AddAsync(userBoard);
                return RedirectToAction("UserBoards");
            }
            return RedirectToAction("UserBoards");
        }
    }
}
