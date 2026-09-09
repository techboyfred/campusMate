namespace UJConnect.Models
{
    public class TutorApplication
    {
        public int ApplicationID { get; private set; }
        public Rate Rate { get; private set; }
        public User Tutor { get; private set; }
        public string Description { get; set; }
        public ModuleRegistration ModuleRegistration { get; private set; }

        //Constructor(s)
        public TutorApplication(User tutor, ModuleRegistration moduleRegistration, Rate rate, string description) {
            this.Tutor = tutor;
            this.Rate = rate;
            this.ModuleRegistration = moduleRegistration;
            this.Description = description;
        }
        public TutorApplication(int applicationID, User tutor, ModuleRegistration moduleRegistration, Rate rate, string description)
        {
            this.ApplicationID = applicationID;
            this.Tutor = tutor;
            this.Rate = rate;
            this.ModuleRegistration = moduleRegistration;
            this.Description = description;
        }

    }
}
