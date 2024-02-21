namespace CodingTracker.Models;

internal class Enums
{
    public enum MenuSelection
    {
        LiveSession,
        ViewRecords,
        AddRecord,
        UpdateRecord,
        DeleteRecord,
        ViewReports,
        CloseApplication
    }

    public enum UpdatingSelection
    {
        UpdateStartTime,
        UpdateEndTime,
        SaveChanges,
        MainMenu
    }

    public enum ReportSelection
    {
        Weekly,
        Monthly,
        Yearly,
        MainMenu
    }
}
