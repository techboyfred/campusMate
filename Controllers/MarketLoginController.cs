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
        private readonly IConfiguration _configuration;

        public MarketLoginController(UserDAO userDAO, IConfiguration configuration)
        {
            _userDAO = userDAO;
            _configuration = configuration;
        }

        //GET: /MarkertLogin/MarketLogin, this will be the empty login form
        [HttpGet]
        public IActionResult MarketLogin()
        {
            ViewData["PrefillUsername"] = TempData["PrefillUsername"];
            return View();
        }

        //POST: /MarketLogin/MarketLogin, this will handle the submitted form
        [HttpPost]
        public IActionResult MarketLogin(string usernameOrStudentEmail, string password)
        {
            if (!_userDAO.AccountExists(usernameOrStudentEmail))
            {
                ViewData["ErrorMessage"] = "No account matches given credentials.";
                return View();
            }
            
            User? user = _userDAO.Login(usernameOrStudentEmail, password);

            if (user == null)
            {
                ViewData["ErrorMessage"] = "The password you entered is incorrect.";
                return View();
            }

            //store important data
            HttpContext.Session.SetInt32("userID", user.UserID);
            HttpContext.Session.SetString("username", user.Username);

            return RedirectToAction("MarketHome", "MarketHome");
        }

        [HttpGet]
        public IActionResult ForgotPassword() 
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(string studentEmail)
        {
            //handle case of invalid student email
            if (!_userDAO.isStudentEmail(studentEmail))
            {
                ViewData["ErrorMessage"] = "Please enter valid student email.";
                return View();
            }

            //handle case of non regustered email
            if (!_userDAO.EmailAlreadyExists(studentEmail))
            {
                ViewData["ErrorMessage"] = "No account matches provided email.";
                return View();
            }

            User? user = _userDAO.GetUserByEmail(studentEmail);

            if (user != null)
            {
                _userDAO.sendResetPasswordLink(
                    user,
                    _configuration["Smtp:Host"],
                   int.Parse( _configuration["Smtp:Port"]),
                    _configuration["Smtp:Username"],
                    _configuration["Smtp:Password"]
                    );
            }

            ViewData["InfoMessage"] = "Reset link has been sent to your student email.";
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string token) 
        {
            User? user = _userDAO.FindUserByResetToken(token);

            if (user == null)
            {
                ViewData["ErrorMessage"] = "This reset link is invalid or has expired";
                return View();
            }

            ViewData["Token"] = token; //carried through so the POST method can rechek it
            return View();
        }

        [HttpPost]
        public IActionResult ResetPassword(string token, string newPassword, string confirmNewPassword)
        {
            //handle non matching passwords
            if (!newPassword.Equals(confirmNewPassword))
            {
                ViewData["ErrorMessage"] = "Passwords do not match";
                ViewData["Token"] = token; //carried through so the POST method can rechek it
                return View();
            }

            User? user = _userDAO.FindUserByResetToken(token);
            if (user == null) 
            {
                ViewData["ErrorMessage"] = "This reset link is invalid or has expired";
            }

            //handle repeated password
            if (!_userDAO.ResetPassword(user, newPassword))
            {
                ViewData["ErrorMessage"] = "New password can't be the same as your old one.";
                ViewData["Token"] = token;
                return View();
            }
            
            TempData["SuccessMessage"] = "Your password has been reset. You can now log in with your new password.";
            return RedirectToAction("MarketLogin", "MarketLogin");
        }

    }
}

