using System.Threading.Tasks;

namespace AprioritWebCalendar.Business.Interfaces
{
    public interface ICalendarImportService
    {
        Task<DomainModels.Calendar> SaveCalendarAsync(DomainModels.Calendar calendar);
        DomainModels.Calendar ConvertIntoDomain(Ical.Net.Calendar iCalendar);
        Ical.Net.Calendar DeserializeCalendar(string icsString);
    }
}
