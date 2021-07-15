using System;
using System.Collections.Generic;
using System.Text;

namespace Tornado.Shared.SmartTable
{
    public class SmartTableParam
    {
        public Pagination Pagination { get; set; }
        public Search Search { get; set; }

        public Sort Sort { get; set; }
    }
}
