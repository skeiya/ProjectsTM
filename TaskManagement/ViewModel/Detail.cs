using System.Xml.Serialization;

namespace TaskManagement.ViewModel
{
    public class Detail
    {
        [XmlIgnore]
        public int CompanyHeight => (int)(CompanyHeightCore * ViewRatio);
        [XmlIgnore]
        public int NameHeight => (int)(NameHeightCore * ViewRatio);
        [XmlIgnore]
        public int RowHeight => (int)(RowHeightCore * ViewRatio);
        [XmlIgnore]
        public int DateWidth => (int)(DateWidthCore * ViewRatio);
        [XmlIgnore]
        public int ColWidth => (int)(ColWidthCore * ViewRatio);
        public int CompanyHeightCore { set; get; } = 10;
        public int NameHeightCore { set; get; } = 10;
        public int RowHeightCore { set; get; } = 10;
        public int DateWidthCore { set; get; } = 50;
        public int ColWidthCore { set; get; } = 20;
        public float ViewRatio { set; get; } = 1.0f;
        [XmlIgnore]
        public float FixedHeight => CompanyHeight + NameHeight + NameHeight;

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
