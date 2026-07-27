using Microsoft.AspNetCore.Mvc;

namespace campusMate.Models
{
    public class RegisterController : Controller
    {
        public IActionResult Register()
        {
            return View();
        }
    }
}
