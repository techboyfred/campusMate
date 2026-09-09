# RoboRumble Technomania Challenge 2026
campusMate: A student hub application for university students to buy and sell products through an online student market, and to connect with other students to find tutors, study partners, and join clubs and societies.
---

## Features

- Online student market
- Apply to be a tutor
- Find a tutor or study partner
- Join official UJ Clubs and Societies
- Lost and found section

---

## Technologies Used
- UI: HTML & CSS
- Backend: c#
- Database: MySQL (hosted on Aiven [https://aiven.io] for now since we don't have the hardware component yet)
- (Temporal) Database Connectivity: MySQL Connector (via NuGet Packages)
- API Server: Raspberry Pi (Flask / Node.js)
- Password Security: BCrypt (via NuGet Packages)
- IDE: Visual Studio (2019)

---
## Project Structure
```
ProductivityTracker/
├── Program.cs
├── README.md
├── docs/
│   ├── FQA_Attendance_Log.md
│   ├── billOfMaterials.xlsx
│   ├── holisticBuild.pdf
│   ├── pitchDeck.pdf
│   ├── designs/
|   |   ├── erd.pdf
|   |   ├── dataUmlDesign.pdf
|   |   └── modelsUmlDesign.pdf
│   └── FQA_Proof/
|       ├── 14-08-26.jpg
|       ├── 14-08-26(2).jpg
|       └── 04-09-26.jpg
├── Models/
│   ├── ActionType.cs
│   ├── Administrator.cs
│   ├── AvailabilitySlot.cs
│   ├── CampusLocation.cs
│   ├── CampusName.cs
│   ├── Club.cs
│   ├── ClubMembership.cs
│   ├── ModerationAction.cs
│   ├── ModuleRegistration.cs
│   ├── Product.cs
│   ├── ProductEnquiry.cs
│   ├── ProductPackage.cs
│   ├── ProductType.cs
│   ├── Rate.cs
│   ├── RatePeriod.cs
│   ├── Report.cs
│   ├── ReportStatus.cs
│   ├── ReportType.cs
│   ├── TutorApplication.cs
│   ├── UJModule.cs
│   └── User.cs
├── Data/
│   ├── ProductDAO.cs
│   ├── ProductPackageDAO.cs
│   ├── ProductEnquiryDAO.cs
│   └── UserDAO.cs
├── Controllers/
│   ├── HomeController.cs
│   ├── MarketLoginController.cs
│   ├── ProfileController.cs
│   └── RegisterController.cs
├── wwwroot/
│   ├── css/
│   |   └── style.css
│   ├── fonts/
│   ├── footer/
│   ├── Header/
│   └── images/
└── Views/
    ├── Admin/
    |   └── Admin.cshtml
    ├── Home/
    |   └── Home.cshtml
    ├── MarketHome/ 
    |   └── MarketHome.cshtml
    ├── MarketLogin/ 
    |   └── MarketLogin.cshtml
    ├── Profile/ 
    |   └── Profile.cshtml
    ├── Register/ 
    |   └── Register.cshtml
    ├── SellerHome/ 
    |   └── SellerHome.cshtml
    ├── Shared/
    |   ├── _Layout.cshtml
    |   ├── _Layout.cshtml.css
    |   └── Error.cshtml
    ├── _ViewStart.cshtml
    └── _ViewImports.cshtml
```

## Main Functionality
1. Login or register an account
2. View all products added to the student market
3. Buyer: Enquire with seller about buying (a) product(s)
4. Buyer: search for a specific product
5. Buyer: Control product views by either filtering or sorting the products by their preferred arrangement
6. Seller: Add products they sell
7. Seller: Specify availability times to simplify meeting arrangements
8. Seller: Confirm meetup with the buyer
9. User in general: Verify or delete account
10. User in general: Report other users and/or products
11. User in general: Apply to be a module tutor
12. User in general: Find tutor(s) and/or study partner(s)
13. User in general: Join clubs and societies
14. Admin: Handle reports
15. Admin: Add new meetup location spots across campuses
16. Admin: View all users and their (sensitive) information
17. Admin: Approve tutor applications
18. Admin: Add new clubs and societies
19. Admin: Change roles of members in clubs
20. System: Automatic validation of products in the market
21. System: Automatic and Admin-handled manual fraud detection
22. Student: Browse through lost and found items and provide hints to prove ownership
23. Student: Post lost items with specific keywords to describe it, and another section which allows the user to describe the item and it matches them with the most accurate but they must also answer more specific questions afterwards to prove ownership.


## Screenshots
