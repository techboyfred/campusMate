using Microsoft.AspNetCore.Mvc;
using UJConnect.Models;
using System;
using System.Collections.Generic;

namespace UJConnect.Controllers
{
    public class SellerController : Controller
    {
        [HttpGet]
        public IActionResult SellerHome()
        {
            // TEMPORARY preview data - swap for real UserDAO/ProductDAO/AvailabilityDAO
            var mockSeller = new User(1, "TestSeller", "12345678@student.uj.ac.za", "unused", DateTime.UtcNow, 0);

            var mockProducts = new List<Product>
            {
                new Product(1, "Instant Noodles (6-pack)", "Chicken flavour, unopened",
                    DateTime.UtcNow, ProductType.GROCERY, mockSeller, "", 45.00m),
                new Product(2, "Scientific Calculator", "Barely used, still has the box",
                    DateTime.UtcNow, ProductType.STATIONERY, mockSeller, "", 180.00m)
            };

            var viewModel = new SellerHomeViewModel
            {
                Username = mockSeller.Username,
                IsVerified = true,
                HasAvailability = true,
                Products = mockProducts,
                Venues = new List<(int, string)>
                {
                    (1, "APK Library Entrance"),
                    (2, "APB Student Centre")
                }
            };

            return View(viewModel);
        }

        // Stub - exists so the availability form's asp-action doesn't throw at render time.
        [HttpPost]
        public IActionResult AddAvailability(string dayOfWeek, string startTime, string endTime, int locationId)
        {
            TempData["InfoMessage"] = "Add Availability isn't wired to the database yet.";
            return RedirectToAction("SellerHome");
        }
    }
}
