using Microsoft.AspNetCore.Mvc;
using UJConnect.Models;
using System;
using System.Collections.Generic;

namespace UJConnect.Controllers
{
    public class AdminController : Controller
    {
        [HttpGet]
        public IActionResult Admin()
        {
            // TEMPORARY preview data - no DAO wiring yet, just enough to view the layout.
            var mockAdmin = new User(99, "AdminUser", "admin@student.uj.ac.za", "unused", DateTime.UtcNow, 0);
            var mockBuyer = new User(1, "lindiwe_k", "lindiwek@student.uj.ac.za", "unused", DateTime.UtcNow, 0);
            var flaggedUser = new User(2, "buyer_thabo22", "thabom@student.uj.ac.za", "unused", DateTime.UtcNow, 5);

            var mockProduct = new Product(1, "Textbook - Calculus 101", "Slightly used, 3rd edition",
                DateTime.UtcNow, ProductType.STATIONERY, flaggedUser, "", 250.00m);

            var viewModel = new AdminDashboardViewModel
            {
                TotalStudentCount = 318,
                ActiveClubCount = 0, // no mock clubs yet - see note below

                FlaggedUsers = new List<User> { flaggedUser },

                UnresolvedReports = new List<Report>
                {
                    new Report(mockBuyer, flaggedUser, "Never showed up to the agreed meetup.", ReportType.NO_SHOW),
                    new Report(mockBuyer, mockProduct, "Listing description doesn't match condition.", ReportType.PRODUCT_ISSUE)
                },

                // Left empty - I don't have your exact TutorApplication constructor (needs Tutor,
                // ModuleRegistration, Rate) confirmed, so guessing risked shipping code that won't
                // compile against your real class. Add a mock instance yourself, or send me the
                // constructor and I'll fill this in.
                PendingTutorApplications = new List<TutorApplication>(),

                Venues = new List<(int, string)>
                {
                    (1, "APK Library Entrance"),
                    (2, "APB Student Centre")
                },

                // Same reasoning as TutorApplications - left empty pending your actual Club constructor.
                Clubs = new List<Club>(),

                SearchQuery = null,
                SearchResults = new List<User>()
            };

            return View(viewModel);
        }

        // --- Stub actions below: exist only so Admin.cshtml's asp-action links don't throw
        //     at render time. Each redirects back with an info banner. Replace with real logic
        //     as you build each feature out. ---

        [HttpPost]
        public IActionResult AddVenue(string campusName, string locationName)
        {
            TempData["InfoMessage"] = "Add Venue isn't wired to the database yet.";
            return RedirectToAction("Admin");
        }

        [HttpPost]
        public IActionResult AddClub(string name, string description, decimal registrationFee, decimal membershipFee, int maxCapacity)
        {
            TempData["InfoMessage"] = "Add Club isn't wired to the database yet.";
            return RedirectToAction("Admin");
        }

        [HttpGet]
        public IActionResult ViewUserReports(int userId)
        {
            TempData["InfoMessage"] = "User report history view isn't built yet.";
            return RedirectToAction("Admin");
        }

        [HttpPost]
        public IActionResult SuspendUser(int userId)
        {
            TempData["InfoMessage"] = "Suspend isn't wired to the database yet.";
            return RedirectToAction("Admin");
        }

        [HttpGet]
        public IActionResult ReportDetail(int id)
        {
            TempData["InfoMessage"] = "Report detail screen isn't built yet.";
            return RedirectToAction("Admin");
        }

        [HttpGet]
        public IActionResult TutorApplicationDetail(int id)
        {
            TempData["InfoMessage"] = "Tutor application detail screen isn't built yet.";
            return RedirectToAction("Admin");
        }

        [HttpGet]
        public IActionResult ResolvedReports()
        {
            TempData["InfoMessage"] = "Resolved reports history isn't built yet.";
            return RedirectToAction("Admin");
        }
    }
}
