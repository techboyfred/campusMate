using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using System.ComponentModel.DataAnnotations;
using UJConnect.Data;
using UJConnect.Models;

namespace UJConnect.Controllers
{
    public class MarketLoginController : Controller
    {
        private readonly UserDAO _userDAO;

        public MarketLoginController(UserDAO userDAO)
        {
            _userDAO = userDAO;
        }
         /*Forgot password page*/
        public IActionResult ForgotPassword()
        {
            return View();
        }


        //GET: /MarkertLogin/MarketLogin, this will be the empty login form
        [HttpGet]
        public IActionResult MarketLogin()
        {
            return View();
        }

        //POST: /MarketLogin/MarketLogin, this will handle the submitted form
        [HttpPost]
        public IActionResult MarketLogin(string username, string password)
        {
            User? user = _userDAO.LoginWithUsername(username, password);


            if (user == null)
            {
                ViewData["ErrorMessage"] = "Invalid username or password.";
                return View();
            }

            //store important data
            HttpContext.Session.SetInt32("userID", user.UserID);
            HttpContext.Session.SetString("username", user.Username);

            return RedirectToAction("MarketHomeController");
        }
    }
}

