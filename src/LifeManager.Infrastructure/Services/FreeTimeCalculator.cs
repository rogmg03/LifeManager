using LifeManager.Application.Common.Interfaces;

namespace LifeManager.Infrastructure.Services;

public class FreeTimeCalculator : IFreeTimeCalculator
{
    public int CalculateEarnedMinutes(int workMinutes, decimal workMinutesPerFreeMinute)
    {
        if (workMinutesPerFreeMinute <= 0) workMinutesPerFreeMinute = 1.0m;
        return (int)Math.Floor(workMinutes / workMinutesPerFreeMinute);
    }
}
