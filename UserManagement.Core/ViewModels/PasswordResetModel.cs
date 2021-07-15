namespace UserManagement.Core.ViewModels
{
    public class PasswordResetModel
    {
        public string UsernameOrEmail { get; set; }
        public int Otp { get; set; }

        public string Password { get; set; }
        public string ComparePassword { get; set; }
    }
}