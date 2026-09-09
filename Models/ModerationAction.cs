namespace UJConnect.Models
{
    public class ModerationAction
    {
        public int ActionID { get; private set; }
        public Report Report { get; private set; }
        public Administrator Admin {  get; private set; }
        public string AdminComment { get; private set; }
        public ActionType ActionType { get; private set; }

        //constructor
        public ModerationAction(Report report, Administrator admin, string adminComment, ActionType actionType) {
            this.Report = report;
            this.Admin = admin;
            this.AdminComment = adminComment;
            this.ActionType = actionType;
        }
        //2nd constr
        public ModerationAction(int actionID, Report report, Administrator admin, string adminComment, ActionType actionType)
        {
            this.ActionID = actionID;
            this.Report = report;
            this.Admin = admin;
            this.AdminComment = adminComment;
            this.ActionType = actionType;
        }
    }
}
