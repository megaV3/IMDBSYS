namespace ELNET_FinalsProject.ViewModels
{
    public class ProfileViewModel
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? ProfileImagePath { get; set; }
        public IFormFile? ProfileImage { get; set; }
        public int CartCount { get; set; }


    }
}
