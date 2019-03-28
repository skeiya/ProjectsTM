using System.Collections.Generic;

namespace TaskManagement
{
    public interface IPeriodCalculator
    {
        int GetTerm(CallenderDay from, CallenderDay to);
        CallenderDay ApplyOffset(CallenderDay to, int offset);
        List<CallenderDay> GetDays(CallenderDay from, CallenderDay to);
    }
}