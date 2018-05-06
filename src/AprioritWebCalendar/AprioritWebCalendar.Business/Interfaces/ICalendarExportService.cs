namespace AprioritWebCalendar.Business.Interfaces
{
    public interface ICalendarExportService
    {
        Ical.Net.Calendar ExportCalendar(DomainModels.Calendar calendar);
        string SerializeCalendar(Ical.Net.Calendar iCalendar);
    }
}
