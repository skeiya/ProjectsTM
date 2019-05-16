using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.ViewModel
{
    public class Detail
    {
        public int CompanyHeight { set; get; } = 20;
        public int NameHeight { set; get; } = 20;
        public int RowHeight { set; get; } = 10;
        public int DateWidth { set; get; } = 60;
        public int ColWidth { set; get; } = 20;
        internal Detail Clone()
        {
            var result = new Detail();
            result.CompanyHeight = this.CompanyHeight;
            result.NameHeight = this.NameHeight;
            result.RowHeight = this.RowHeight;
            result.DateWidth = this.DateWidth;
            result.ColWidth = this.ColWidth;
            return result;
        }
    }
}
