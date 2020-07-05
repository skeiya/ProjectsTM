using System.Xml.Serialization;

namespace ProjectsTM.ViewModel
{
    public class Detail
    {
        public int CompanyHeightCore { set; get; } = 10;
        public int NameHeightCore { set; get; } = 10;
        public int RowHeightCore { set; get; } = 10;
        public int DateWidthCore { set; get; } = 50;
        public int ColWidthCore { set; get; } = 20;
        public float ViewRatio { set; get; } = 1.0f;

        internal Detail Clone()
        {
            var result = new Detail();
            result.CompanyHeightCore = this.CompanyHeightCore;
            result.NameHeightCore = this.NameHeightCore;
            result.RowHeightCore = this.RowHeightCore;
            result.DateWidthCore = this.DateWidthCore;
            result.ColWidthCore = this.ColWidthCore;
            result.ViewRatio = this.ViewRatio;
            return result;
        }
    }
}
