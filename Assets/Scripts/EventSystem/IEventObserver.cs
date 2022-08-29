public interface IEventObserver
{
    void OnEvent(EventId id, object payload);
}