using Booking.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Booking.Core.ViewModels
{

    public class TripDaysRequestModel : IValidatableObject
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public bool Monday { get; set; }
        [Required]
        public bool Tuesday { get; set; }
        [Required]
        public bool Wednesday { get; set; }
        [Required]
        public bool Thursday { get; set; }
        [Required]
        public bool Friday { get; set; }
        [Required]
        public bool Saturday { get; set; }
        [Required]
        public bool Sunday { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(Title))
            {
                yield return new ValidationResult("Title cannot be null or empty");
            }
        }
    }
    public class TripDaysViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }

      
    }

    public class TripDaysDetailViewModel : TripDaysViewModel
    {
        public Guid? CreatedBy { get; set; }

        public Guid? ModifiedBy { get; set; }

        public DateTime ModifiedOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public static explicit operator TripDaysDetailViewModel(TripDays source)
        {
            return new TripDaysDetailViewModel
            {
                Friday = source.Friday,
                Monday = source.Monday,
                Saturday = source.Saturday,
                Sunday = source.Sunday,
                Thursday = source.Thursday,
                Tuesday = source.Tuesday,
                Title = source.Title,
                Wednesday = source.Wednesday,
                CreatedBy = source.CreatedBy,
                ModifiedBy = source.ModifiedBy,
                ModifiedOn = source.ModifiedOn.HasValue ? source.ModifiedOn.Value : default,
                CreatedOn = source.CreatedOn
            };
        }


    }
    public class TripDaysEditViewModel : TripDaysViewModel
    {
        
    }


}
