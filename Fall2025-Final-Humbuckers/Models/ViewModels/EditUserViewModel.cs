namespace Fall2025_Final_Humbuckers.Models.ViewModels
{
    public class EditUserViewModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public List<string> CurrentRoles { get; set; } = new List<string>();
        public List<string> AllRoles { get; set; } = new List<string>();
    }
}
