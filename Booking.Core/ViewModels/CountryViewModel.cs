using Booking.Core.Dtos;
using Booking.Core.Models;
using System;
using System.ComponentModel.DataAnnotations;
using Tornado.Shared.ViewModels;

namespace Booking.Core.ViewModels
{

    public class CountryCreateModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public string Code { get; set; }
        //protected internal string Id { get; internal set; }
        public  string Id { get;  set; }
        public string DialingCode { get; set; }

    }

    public class CountryEditModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public string Code { get; set; }
        public string Id { get; set; }
        public string DialingCode { get; set; }

    }

    public class CountryViewModel : BaseViewModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public string Code { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
        public long RowNo { get; set; }
        public string DialingCode { get; set; }
        public static explicit operator CountryViewModel(CountryDto source)
        {
            var destination = new CountryViewModel
            {
                Id = source.Id.ToString(),
                Code = source.Code,
                Name = source.Name,
                Description = source.Description,
                CreatedBy = source.CreatedBy,
                TotalCount = source.TotalCount,
                RowNo = source.RowNo
            };
            return destination;
        }

        public static explicit operator CountryViewModel(Country source)
        {
            var destination = new CountryViewModel
            {
                Id = source.Id.ToString(),
                Code = source.Code,
                Name = source.Name, 
                CreatedBy = source.CreatedBy,
                ModifiedBy = source.ModifiedBy,
                //Databases
                DialingCode = source.DialingCode
            };
            return destination;
        }
    }
}