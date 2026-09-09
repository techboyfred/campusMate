using Microsoft.AspNetCore.Mvc;

namespace UJConnect.Controllers
{
    public class MarketHomeController : Controller
    {
        [HttpGet]
        public IActionResult MarketHome()
        {
            return View();
        }
        public IActionResult MarketProduct()
        {
            return View();
        }
        public IActionResult MarketShop()
        {
            return View();
        }
        public IActionResult MarketCart()
        {
            return View();
        }
        public IActionResult MarketContact()
        {
            return View();
        }

    }
}