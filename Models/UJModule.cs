namespace UJConnect.Models
{
    public class UJModule
    {
        public string ModuleCode { get; private set; }
        public string ModuleName { get; private set; }

        //Constructor(s)
        public UJModule(string moduleCode, string moduleName) {
            this.ModuleCode = moduleCode;
            this.ModuleName = moduleName;
        }
    }
}
