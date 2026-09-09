using Microsoft.AspNetCore.Mvc;
using UJConnect.Data;
using UJConnect.Models;

namespace UJConnect.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserDAO _userDAO;

        public RegisterController(UserDAO userDAO)
        {
            _userDAO = userDAO;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string username, string studentEmail, string password, string confirmPassword) 
        {
            //hande invalid student email
            if (!_userDAO.isStudentEmail(studentEmail))
            {
                ViewData["ErrorMessage"] = "Invalid student email";
                return View();
            }
            
            //handle non matching passwords
            if (!password.Equals(confirmPassword))
            {
                ViewData["ErrorMessage"] = "Passwords do not match";
                return View();
            }

            //handle duplicate username and/or student email
            if (_userDAO.EmailAlreadyExists(studentEmail))
            {
                ViewData["ErrorMessage"] = "An account with that email already exists.";
                return View();
            }

            if (_userDAO.UsernameAlreadyExists(username))
            {
                ViewData["ErrorMessage"] = "That username is already taken.";
                return View();
            }

            User newUser = new User(username, studentEmail, password);
            bool isRegistered = _userDAO.Register(newUser);

            if (isRegistered == false)
            {
                ViewData["ErrorMessage"] = "Error-- couldn't register account.";
                return View();
            }

            //store username for easy login
            TempData["PrefillUsername"] = newUser.Username;

            return RedirectToAction("MarketLogin", "MarketLogin");
        }
    }
}
