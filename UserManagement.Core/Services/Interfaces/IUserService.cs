using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Tornado.Shared.Models;
using Tornado.Shared.ViewModels;
using UserManagement.Core.ViewModels;

namespace UserManagement.Core.Services.Interfaces
{
    public interface IUserService
    {
        Task AssignDefaultPermission(GigmUser user, string role);
        Task<string> GenerateEmailConfirmationToken(string userId);
        Task<int?> GeneratePhoneConfirmationOTP(string usernameOrEmail);

        Task<List<ValidationResult>> ConfirmAccountPhoneNumber(string username, int Otp);
        Task<List<ValidationResult>> ConfirmAccountEmail(string email, string token);
        int GenerateUserOTP(GigmUser user);
        bool VerifyUserOTP(GigmUser user, int otp);
        Task<GigmUser> FindUser(string usernameOrEmail);

        Task<List<ValidationResult>> UpdateStaffUser(EditStaffViewModel model);
        Task<List<ValidationResult>> UpdateCaptainUser(EditCaptainViewModel model);
        Task<List<ValidationResult>> UpdateBusBoyUser(EditBusBoyViewModel model);
        Task<List<ValidationResult>> UpdatePartnerUser(EditPartnerViewModel model);
        Task<List<ValidationResult>> UpdateCustomerUser(EditUserViewModel model);

        Task<List<ValidationResult>> DeleteCaptain(string userId);
        Task<List<ValidationResult>> DeleteCustomer(string userId);
        Task<List<ValidationResult>> DeleteStaff(string userId);
        Task<List<ValidationResult>> DeleteBusBoy(string userId);
        Task<List<ValidationResult>> DeletePartner(string userId);

        Task<List<ValidationResult>> CreateStaff(SetupStaffViewModel model);
        Task<List<ValidationResult>> CreatePartner(SetupPartnerViewModel model);
        Task<List<ValidationResult>> CreateBusBoy(SetupBusBoyViewModel model);
        Task<List<ValidationResult>> CreateCaptain(SetupCaptainViewModel model);
        Task<List<ValidationResult>> CreateCustomerUser(CustomerRegisterViewModel model);

        Task<List<StaffListViewModel>> GetAllStaff(BaseSearchViewModel model, out int totalCount);
        Task<List<PartnerListViewModel>> GetAllPartner(BaseSearchViewModel model, out int totalCount);
        Task<List<CustomerListViewModel>> GetAllCustomer(BaseSearchViewModel model, out int totalCount);
        Task<List<CaptainListViewModel>> GetAllCaptain(BaseSearchViewModel model, out int totalCount);
        Task<List<BusBoyListViewModel>> GetAllBusBoy(BaseSearchViewModel model, out int totalCount);

        Task<StaffViewModel> GetStaffById(string userId);
        Task<UserViewModel> GetCustomerById(string userId);
        Task<BusBoyViewModel> GetBusBoyById(string userId);
        Task<CaptainViewModel> GetCaptainById(string userId);
        Task<PartnerViewModel> GetPartnerById(string userId);
        Task<List<ValidationResult>> ChangePassword(ChangePasswordViewModel model);
        Task<(List<ValidationResult> validationResults, string path)>
            CreateProfilePicture(PictureCreateViewModel model);
        Task<(List<ValidationResult> errors, UserResetPasswordModel userOtpModel)>
            GeneratePasswordResetOTP(string usernameOrEmail);
        Task<List<ValidationResult>> ResetPassword(PasswordResetModel model);
        Task<List<ValidationResult>> CreateOrUpdatePin(PinCreateModel model);
        Task<bool> VerifyPin(string pin);
    }
}