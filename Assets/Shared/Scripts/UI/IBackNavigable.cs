namespace Shared.UI
{
    /// <summary>A screen or panel that the Back button (Android Back / Esc) can close.</summary>
    public interface IBackNavigable
    {
        bool IsOpen { get; }
        void Close();
    }
}