using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Dtos;

namespace Booking.Core.Dtos
{
    public class RouteDto : BaseDto
    {
        public string Name { get; set; }
        public int RouteNo { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public long RowNo { get; set; }

        public Guid? ModifiedBy { get; set; }
        public Guid? CreatedBy { get; set; }

        public decimal BaseFee { get; set; }
        public decimal DispatchFee { get; set; }
    }
}
