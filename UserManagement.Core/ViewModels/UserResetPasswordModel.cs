namespace UserManagement.Core.ViewModels
{
    public class UserResetPasswordModel
    {
        public int Otp { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string DialingCode { get; set; }
    }
}