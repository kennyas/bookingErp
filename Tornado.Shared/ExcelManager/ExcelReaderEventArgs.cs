using System;
using System.Collections.Generic;
using System.Text;
namespace ExcelManager
{
    public class ExcelReaderEventArgs : EventArgs
    {
        public ExcelReaderSheet Sheet
        {
            get;
            set;
        }

        public ExcelReaderEventArgs()
        {
        }

        public ExcelReaderEventArgs(ExcelReaderSheet sheet)
        {
            this.Sheet = sheet;
        }
    }
}

