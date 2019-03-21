using TaskManagement;

namespace UnitTestProject1
{
    internal class DummyPeriodCalculator : IPeriodCalculator
    {
        public int GetTerm(CallenderDay from, CallenderDay to)
        {
            return to.Day - from.Day + 1;
        }
    }
}