using Microsoft.AspNetCore.Mvc;

namespace Tofft_preprod.Controllers
{
    public class BoardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Taskboard()
        {
            return View();
        }

        [HttpGet]
        public IActionResult UserBoards()
        {
            return View();
        }
    }
}
