namespace IMDBSYS.ViewModels.Admin
{
    public class UserManagementViewModel
    {
        // Unique security identifier key from your DB
        public int Id { get; set; }

        // Core profile fields
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Security role level (e.g., "Admin" or "User")
        public string Role { get; set; } = string.Empty;
    }
}
