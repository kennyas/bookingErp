using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Tornado.Shared.Enums;
using UserManagement.Core.Enums;

namespace UserManagement.Core.ViewModels
{
    public class SetupUserViewModel
    {
        [RegularExpression(@"^\S+$", ErrorMessage = "LastName: Enter non-spaced character only")]
        public string LastName { get; set; }
        [RegularExpression(@"^\S+", ErrorMessage = "FirstName: Enter non-spaced character only")]
        [Required(ErrorMessage = "Firstname is required")]
        public string FirstName { get; set; }
        [RegularExpression(@"^\S+", ErrorMessage = "MiddleName: Enter non-spaced character only")]
        public string MiddleName { get; set; }

        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Email: not a valid Email")]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required]
        public Gender? Gender { get; set; }

        [JsonIgnore]
        public string Password { get; internal set; }
        [RegularExpression(@"^[0]\d{10}$", ErrorMessage = "PhoneNumber: Can only be 11 digit character")]
        [Required(ErrorMessage = "Phone Number is required")]
        public string PhoneNumber { get; set; }
        public Guid Id { get; internal set; }
        public string DialingCode { get; set; }
    }

    public class SetupStaffViewModel : SetupUserViewModel
    {
        [Required(ErrorMessage = "DepartmentId is required")]
        public string DepartmentId { get; set; }
        [Required(ErrorMessage = "EmployeeCode is required")]
        public string EmployeeCode { get; set; }
        public string Unit { get; set; }
    }

    public class SetupPartnerViewModel : SetupUserViewModel
    {
        [RegularExpression(@"^[0]\d{10}$", ErrorMessage = "PartnerPhoneNumber: Can only be 11 digit character")]
        public string PartnerPhoneNumber { get; set; }
        public string PartnerAddress { get; set; }

        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "PartnerEmail: not a valid Email")]
        public string PartnerEmail { get; set; }
    }

    public class SetupCaptainViewModel : SetupUserViewModel
    {
        [Required(ErrorMessage = "EmployeeCode is required")]
        public string EmployeeCode { get; set; }
        public CaptainStatus? Status { get; set; }
    }

    public class SetupBusBoyViewModel : SetupUserViewModel
    {
        public BusBoyStatus? Status { get; set; }
    }
}