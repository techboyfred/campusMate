namespace UJConnect.Models
{
    public class Rate
    {
        public int RateID { get; private set; }
        public RatePeriod RatePeriod { get; private set; }
        public decimal Price { get; set; }

        //Constructr(s)
        public Rate(RatePeriod ratePeriod, decimal price) {
            this.RatePeriod = ratePeriod;
            this.Price = price;
        }
        public Rate(int rateID, RatePeriod ratePeriod, decimal price)
        {
            this.RateID = rateID;
            this.RatePeriod = ratePeriod;
            this.Price = price;
        }
    }
}
