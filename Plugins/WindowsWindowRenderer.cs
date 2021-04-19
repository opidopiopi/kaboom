using Kaboom.Application;
using Kaboom.Domain.WindowTree.ArrangementAggregate;
using System;

namespace Plugins
{
    public class WindowsWindowRenderer : IWindowRenderer
    {
        private WindowMapper m_mapper;

        public WindowsWindowRenderer(WindowMapper mapper)
        {
            m_mapper = mapper;
        }

        public void Render(Window window)
        {
            Win32Wrapper.MoveWindow(
                m_mapper.MapToWindowHandle(window.ID),
                window.Bounds.X,
                window.Bounds.Y,
                window.Bounds.Width,
                window.Bounds.Height,
                true
            );
        }
    }
}
