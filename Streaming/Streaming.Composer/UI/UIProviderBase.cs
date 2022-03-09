namespace Streaming.Composer.UI
{
    public abstract class UIProviderBase : IUIProviderBase
    {
        public string Key { get; set; }
        public string Title { get; set; }
        public string EntryKey { get; set; }
    }
}
