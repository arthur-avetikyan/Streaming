namespace Streaming.Composer.UI
{
    public interface IUIProviderBase
    {
        string Key { get; }
        string Title { get; }
        string EntryKey { get; }
    }
}
