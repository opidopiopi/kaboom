namespace Kaboom.Model
{
    public delegate void NewWindowCallback(IWindowIdentity identity, Abstract.Rectangle bounds);
    public delegate void RemoveWindowCallback(IWindowIdentity identity);

    public interface IProvideWindows
    {
        void SetNewWindowCallback(NewWindowCallback callback);
        void SetRemoveWindowCallback(RemoveWindowCallback callback);
    }
}
