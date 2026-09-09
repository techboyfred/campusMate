using System.Collections.Generic;

namespace UJConnect.Models
{
    public class MarketHomeViewModel
    {
        public string Username { get; set; } = string.Empty;
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
