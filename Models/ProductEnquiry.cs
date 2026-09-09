namespace UJConnect.Models
{
    public class ProductEnquiry
    {
        public int ProductEnquiryID { get; private set; }
        public User Buyer { get; private set; }
        public Product Product { get; private set; }
        public AvailabilitySlot AvailabilitySlot { get; private set; }
        public DateTime EnquiredAt { get; private set; }
        public bool MeetupConfirmed {  get; set; }

        //Constructor(s)
        public ProductEnquiry(User buyer, Product product, AvailabilitySlot availabilitySlot) {
            this.Buyer = buyer;
            this.Product = product;
            this.AvailabilitySlot = availabilitySlot;
            this.EnquiredAt = DateTime.UtcNow;
            this.MeetupConfirmed = false;
        }
        public ProductEnquiry(int productEnquiryID, User buyer, Product product, AvailabilitySlot availabilitySlot, bool meetupConfirmed, DateTime enquiredAt)
        {
            this.ProductEnquiryID = productEnquiryID;
            this.Buyer = buyer;
            this.Product = product;
            this.AvailabilitySlot = availabilitySlot;
            this.EnquiredAt = enquiredAt;
            this.MeetupConfirmed = meetupConfirmed;
        }
    }
}
