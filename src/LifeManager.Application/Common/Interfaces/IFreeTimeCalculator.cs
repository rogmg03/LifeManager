namespace LifeManager.Application.Common.Interfaces;

public interface IFreeTimeCalculator
{
    /// <summary>
    /// Returns how many free minutes are earned for the given number of work minutes
    /// based on the user's configured ratio.
    /// </summary>
    int CalculateEarnedMinutes(int workMinutes, decimal workMinutesPerFreeMinute);
}
