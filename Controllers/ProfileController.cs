using Microsoft.AspNetCore.Mvc;

namespace UJConnect.Controllers
{
    public class ProfileController : Controller
    {
        // =========================================================
        // MY PROFILE
        // =========================================================

        [HttpGet]
        public IActionResult MyProfile()
        {
            return View();
        }


        // =========================================================
        // EDIT PROFILE
        // =========================================================

        [HttpGet]
        public IActionResult EditProfile()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProfile(
            string Username,
            string DisplayName,
            string Bio)
        {
            // Database update will be connected here later.

            TempData["SuccessMessage"] =
                "Your profile has been updated successfully.";

            return RedirectToAction(nameof(MyProfile));
        }


        // =========================================================
        // MY PURCHASES
        // =========================================================

        [HttpGet]
        public IActionResult MyPurchase()
        {
            return View();
        }


        // =========================================================
        // MY LISTINGS
        // =========================================================

        [HttpGet]
        public IActionResult MyListing()
        {
            return View();
        }


        // =========================================================
        // NOTIFICATIONS
        // =========================================================

        [HttpGet]
        public IActionResult Notifications()
        {
            return View();
        }


        // =========================================================
        // CHANGE PASSWORD
        // =========================================================

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(
            string CurrentPassword,
            string NewPassword,
            string ConfirmPassword)
        {
            // Check current password

            if (string.IsNullOrWhiteSpace(CurrentPassword))
            {
                TempData["ErrorMessage"] =
                    "Please enter your current password.";

                return View();
            }


            // Check new password

            if (string.IsNullOrWhiteSpace(NewPassword))
            {
                TempData["ErrorMessage"] =
                    "Please enter a new password.";

                return View();
            }


            // Check password confirmation

            if (NewPassword != ConfirmPassword)
            {
                TempData["ErrorMessage"] =
                    "The new passwords do not match.";

                return View();
            }


            // =====================================================
            // DATABASE PASSWORD UPDATE WILL GO HERE
            // =====================================================

            TempData["SuccessMessage"] =
                "Your password has been changed successfully.";

            return RedirectToAction(nameof(MyProfile));
        }


        // =========================================================
        // VERIFY ACCOUNT
        // =========================================================

        [HttpGet]
        public IActionResult VerifyAccount()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult VerifyAccount(string verificationCode)
        {
            // Actual account verification will be connected
            // to the database later.

            TempData["SuccessMessage"] =
                "Your verification request has been submitted.";

            return RedirectToAction(nameof(MyProfile));
        }


        // =========================================================
        // LOGOUT
        // =========================================================

        [HttpGet]
        public IActionResult Logout()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LogoutConfirmed()
        {
            // Clear the current user's session.

            HttpContext.Session.Clear();

            // Return to the login page.

            return RedirectToAction(
                "MarketLogin",
                "MarketLogin"
            );
        }


        // =========================================================
        // DELETE ACCOUNT
        // =========================================================

        [HttpGet]
        public IActionResult DeleteAccount()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAccountConfirmed()
        {
            // =====================================================
            // ACTUAL DATABASE ACCOUNT DELETION WILL GO HERE
            // =====================================================

            // Clear the user's session.

            HttpContext.Session.Clear();

            TempData["SuccessMessage"] =
                "Your CampusMate account has been deleted.";

            // Return to login.

            return RedirectToAction(
                "MarketLogin",
                "MarketLogin"
            );
        }
    }
}