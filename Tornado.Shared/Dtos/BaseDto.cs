using System;
using System.Collections.Generic;
using System.Text;

namespace Tornado.Shared.Dtos
{
    public class BaseDto
    {
        public Guid Id { get; set; }
        public int TotalCount { get; set; }
    }
}
