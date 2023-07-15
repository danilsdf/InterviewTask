using Microsoft.AspNetCore.Mvc;

namespace SlotMachine.Controllers
{
    public class PlayerController : Controller
    {
        public IActionResult Index()
        {
            return Ok();
        }
    }
}
