namespace AprioritWebCalendar.Business.Interfaces
{
    public interface ICacheService
    {
        void SetItem<TItem>(string key, TItem item, int timeToStoreMinutes);
        TItem GetItem<TItem>(string key);
        void RemoveItem(string key);
    }
}
