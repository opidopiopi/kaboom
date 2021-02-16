namespace Kaboom.Model
{
    public delegate void WindowCallback(Window window);

    public interface IProvideWindows
    {
        void SetNewWindowCallback(WindowCallback callback);
        void SetRemoveWindowCallback(WindowCallback callback);
    }
}
