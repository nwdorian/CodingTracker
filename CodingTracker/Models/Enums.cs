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
        CloseApplication
    }

    public enum UpdatingSelection
    {
        StartTime,
        EndTime,
        SaveChanges,
        MainMenu
    }
}
