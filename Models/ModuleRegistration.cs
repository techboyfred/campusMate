namespace UJConnect.Models
{
    public class ModuleRegistration
    {
        public int RegistrationID { get; private set; }
        public string ModuleCode { get; private set; }
        public User User { get; private set; }
        public int YearEnrolled { get; private set; }
        public int FinalMark { get; private set; }
        public string FilePath { get; private set; }

        //Constructor(s)
        public ModuleRegistration(User user, string moduleCode, int yearEnrolled, int finalMark, string filePath) {
            this.User = user;
            this.ModuleCode = moduleCode;
            this.YearEnrolled = yearEnrolled;
            this.FinalMark = finalMark;
            this.FilePath = filePath;
        }
        //constructor for accessing 
        public ModuleRegistration(int registrationID, User user, string moduleCode, int yearEnrolled, int finalMark, string filePath)
        {
            this.RegistrationID = registrationID;
            this.User = user;
            this.ModuleCode = moduleCode;
            this.YearEnrolled = yearEnrolled;
            this.FinalMark = finalMark;
            this.FilePath = filePath;
        }
    }
}
