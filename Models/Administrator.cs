namespace UJConnect.Models
{
    public class Administrator
    {
        public User Admin { get; private set; }
        public DateTime DateAppointed { get; private set; }

        //Constructor
        public Administrator(User admin) {
            this.Admin = admin;
            this.DateAppointed = DateTime.UtcNow;
        }
        //Constructor for rebuilding object from database
        public Administrator(User admin, DateTime dateAppointed)
        {
            this.Admin = admin;
            this.DateAppointed = dateAppointed;
        }
    }
}
