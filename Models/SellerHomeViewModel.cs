using System.Collections.Generic;

namespace UJConnect.Models
{
    public class SellerHomeViewModel
    {
        public string Username { get; set; } = string.Empty;
        public bool IsVerified { get; set; }
        public bool HasAvailability { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
        public List<CampusLocation> Venues { get; set; } = new List<CampusLocation>();
    }
}
