using Booking.Core.Dtos;
using Booking.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Tornado.Shared.ViewModels;

namespace Booking.Core.ViewModels
{
    public class StateCreateModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string CountryId { get; set; }
        //protected internal Guid Id { get; internal set; }

    }

    public class StateEditModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public Guid CountryId { get; set; }
        public string Id { get; set; }
    }

    public class StateViewModel : BaseViewModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public string Code { get; set; }

        [Required]
        public Guid? CountryId { get; set; }

        public Guid? ModifiedBy { get; set; }

        public Guid? CreatedBy { get; set; }

        public string Country { get; set; }

        public long RowNo { get; set; }

        public string StateCode { get; set; }
        public string CountryCode { get; set; }

        public static explicit operator StateViewModel(StateDto source)
        {
            var destination = new StateViewModel()
            {
                Name = source.Name,
                Id = source?.Id.ToString(),
                Code = source.Code,
                Description = source.Description,
                CountryId = source.CountryId,
                CreatedId = source.CreatedBy == null ? Guid.Empty : source.CreatedBy.Value,
                CreatedBy = source.CreatedBy,
                TotalCount = source.TotalCount,
                RowNo = source.RowNo,
                Country = source.Country
            };

            return destination;
        }

        public static explicit operator StateViewModel(State source)
        {
            var destination = new StateViewModel()
            {
                Name = source.Name,
                Id = source.Id.ToString(),
                Code = source.Code,
                Description = source.Description,
                CreatedBy = source.CreatedBy,
                ModifiedBy = source.ModifiedBy               
            };

            return destination;
        }
    }
}
