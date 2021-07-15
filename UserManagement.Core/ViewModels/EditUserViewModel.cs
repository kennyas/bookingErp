using System.ComponentModel.DataAnnotations;
using Tornado.Shared.Enums;
using UserManagement.Core.Enums;

namespace UserManagement.Core.ViewModels
{
    public class EditUserViewModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }

        [EmailAddress(ErrorMessage = "Email address isn't valid")]
        [Required(ErrorMessage = "Email address is required")]
        public string Email { get; set; }

        [Required]
        public Gender? Gender { get; set; }

        public string PhoneNumber { get; set; }
    }

    public class EditCaptainViewModel : EditUserViewModel
    {
        public CaptainStatus? Status { get; set; }
        public string EmployeeCode { get; set; }
    }

    public class EditBusBoyViewModel : EditUserViewModel { 
        public BusBoyStatus? Status { get; set; }
    }

    public class EditStaffViewModel : EditUserViewModel
    {
        public string Unit { get; set; }
        public string DepartmentId { get; set; }
        public string EmployeeCode { get; set; }
    }

    public class EditPartnerViewModel : EditUserViewModel
    {
        public string PartnerAddress { get; set; }
        public string PartnerEmail { get; set; }
        public string PartnerPhoneNumber { get; set; }
    }
}