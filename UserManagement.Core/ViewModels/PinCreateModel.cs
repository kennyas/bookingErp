using System.ComponentModel.DataAnnotations;

namespace UserManagement.Core.ViewModels
{
    public class PinCreateModel
    {
        [RegularExpression(@"^\d{4}$", ErrorMessage = "Pin is 4 digit numbers")]
        public string OldPin { get; set; }

        [RegularExpression(@"^\d{4}$", ErrorMessage = "Pin is 4 digit numbers")]
        [Required]
        public string NewPin { get; set; }
    }


    public class PinVerifyModel
    {
        [RegularExpression(@"^\d{4}$", ErrorMessage = "Pin is 4 digit numbers")]
        [Required]
        public string Pin { get; set; }
    }
}