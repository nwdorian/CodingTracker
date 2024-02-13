namespace CodingTracker.Models;

internal class Coding
{
    public int Id { get; set; }
    public string? StartTime { get; set; }
    public string? EndTime { get; set; }
    public string? Duration
    {
        get
        {
            return CalculateDuration();
        }
    }

    private string? CalculateDuration()
    {
        DateTime start = DateTime.Parse(StartTime);
        DateTime end = DateTime.Parse(EndTime);

        TimeSpan duration = end - start;

        return string.Format("{0:%d} days, {0:%h} hours, {0:%m} minutes", duration);
    }
}
