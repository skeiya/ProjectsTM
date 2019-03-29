using System.Collections.Generic;
using TaskManagement;

namespace UnitTestProject1
{
    internal class DummyPeriodCalculator : IPeriodCalculator
    {
        public CallenderDay ApplyOffset(CallenderDay from, int offset)
        {
            throw new System.NotImplementedException();
        }

        public List<CallenderDay> GetDays(CallenderDay from, CallenderDay to)
        {
            throw new System.NotImplementedException();
        }

        public int GetOffset(CallenderDay from, CallenderDay to)
        {
            throw new System.NotImplementedException();
        }

        public int GetTerm(CallenderDay from, CallenderDay to)
        {
            return to.Day - from.Day + 1;
        }
    }
}