using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Tofft_preprod.Controllers
{
    [Authorize(Policy = "RequireUser")]
    public class BoardController : Controller
    {
        [HttpGet]
        public IActionResult Taskboard()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult CreateBoard(){
            return View();
        }

        [HttpGet]
        public IActionResult UserBoards()
        {
            return View();
        }
    }
}
