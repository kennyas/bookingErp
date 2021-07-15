using Booking.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using Tornado.Shared.Models;

namespace Booking.Core.Models
{
    public class Hike : BaseEntity
    {
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public DayOfWeek DayOfWeek { get; set; }

        public decimal? Increase { get; set; }

        public IncreaseType IncreaseType { get; set; }

        public HikeType HikeType { get; set; }
        [MaxLength(100)]
        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }
    }
}
