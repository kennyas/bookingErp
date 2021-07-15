using System.ComponentModel.DataAnnotations;
using Tornado.Shared.Models;

namespace Booking.Core.Models
{
    public class Country : BaseEntity
    {
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(300)]
        public string Description { get; set; }
        [MaxLength(5)]
        public string Code { get; set; }        
        public string Currency { get; set; }
        [MaxLength(10)]
        public string DialingCode { get; set;  }
    }
}
