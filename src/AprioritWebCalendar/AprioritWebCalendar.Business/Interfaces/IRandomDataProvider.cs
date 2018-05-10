namespace AprioritWebCalendar.Business.Interfaces
{
    public interface IRandomDataProvider
    {
        string GetRandomBase64String(int bytesSize);
    }
}
