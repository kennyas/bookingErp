using System;
using System.Collections.Generic;
using System.Text;

namespace Tornado.Shared.SmartTable
{
    public class Pagination
    {
        public int Start { get; set; }

        public int TotalItemCount { get; set; }

        public int Number { get; set; }

        public int NumberOfPages { get; set; }
    }
}
