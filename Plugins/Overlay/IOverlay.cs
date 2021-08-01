namespace Plugins.Overlay
{
    public interface IOverlay
    {
        void AddComponent(IOverlayComponent component);
        void RemoveComponent(IOverlayComponent component);

        void ReRender();
    }
}