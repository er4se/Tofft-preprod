using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Tofft_preprod.Models;
using Tofft_preprod.DbContext;
using Microsoft.AspNetCore.Identity;
using Tofft_preprod.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Tofft_preprod.Repositories;
using System.Diagnostics;

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

        [HttpGet]
        [Authorize(Policy = "BoardMember")]
        public async Task<IActionResult> Details(string id) //index 
        {
            var user = await _userManager.GetUserAsync(User);
            var viewModel = new BoardViewModel();

            var board = await _repository.GetByIdAsync(id);

            viewModel.Board = board;

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Index() //userboards
        {
            var user = await _userManager.GetUserAsync(User);
            var viewModel = new BoardViewModel();

            viewModel.UserBoards = await _context.UserToBoards
                .Where(utb => utb.UserId == user.Id)
                .Select(utb => utb.Board)
                .ToListAsync();

            return View(viewModel);
        }

        #region Create

        [HttpGet]
        public IActionResult Create(){
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(BoardDTO dto)
        {
            var user = await _userManager.GetUserAsync(User);
            var board = new Board(dto);

            board.AdminId = user.Id;

            if (ModelState.IsValid)
            {
                await _repository.CreateAsync(board);
                return RedirectToAction("Index");
            }

            return RedirectToAction("Error");
        }

        [HttpPost]
        public async Task<IActionResult> CreateOLD([Bind(Prefix = "Board")] Board board)
        {
            ModelState.Remove("Board.Id");
            ModelState.Remove("Board.AdminId");
            ModelState.Remove("Board.UserToBoards");
            ModelState.Remove("Board.Missions");

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
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        #endregion
    }
}
