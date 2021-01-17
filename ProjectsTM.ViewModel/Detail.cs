namespace ProjectsTM.ViewModel
{
    public class Detail
    {
        public int CompanyHeightCore { get; set; } = 10;
        public int NameHeightCore { get; set; } = 10;
        public int RowHeightCore { get; set; } = 10;
        public int DateWidthCore { get; set; } = 50;
        public int ColWidthCore { get; set; } = 20;
        public float ViewRatio { get; set; } = 1.0f;

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
