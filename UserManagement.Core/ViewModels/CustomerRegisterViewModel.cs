using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Tornado.Shared.Enums;

namespace UserManagement.Core.ViewModels
{
    public class CustomerRegisterViewModel
    {
        [Required(ErrorMessage = "Dialing code is required")]
        public string DialingCode { get; set; }
        [Required(ErrorMessage = "PhoneNumber is required")]
        [RegularExpression(@"^[0]\d{10}$", ErrorMessage = "PhoneNumber: Can only be 11 digit character")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Email is not in the required format")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [StringLength(30, MinimumLength = 6, ErrorMessage ="Password: must have a minimum of 6 character")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Firstname is required")]
        [RegularExpression(@"^\S+$", ErrorMessage = "FirstName: Enter non-spaced character only")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Gender is required")]
        public Gender Gender { get; set; }
        [RegularExpression(@"^\S+$", ErrorMessage = "LastName: Enter non-spaced character only")]
        public string LastName { get; set; }
        [JsonIgnore]
        protected internal Guid Id { get; internal set; } = Guid.Empty;
    }
}