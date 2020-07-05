using FreeGridControl;

namespace ProjectsTM.UI
{
    static class WorkItemGridConstants
    {
        public static int FixedRows = 3;
        public static int FixedCols = 3;

        public static ColIndex YearCol { get; } = new ColIndex(0);
        public static ColIndex MonthCol { get; } = new ColIndex(1);
        public static ColIndex DayCol { get; } = new ColIndex(2);

        public static RowIndex CompanyRow { get; } = new RowIndex(0);
        public static RowIndex LastNameRow { get; } = new RowIndex(1);
        public static RowIndex FirstNameRow { get; } = new RowIndex(2);
    }
}
