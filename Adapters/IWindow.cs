using System;
using System.Drawing;

namespace Kaboom.Adapters
{
    public interface IWindow
    {
        IntPtr WindowHandle { get; }

        string WindowName();
        Rectangle GetActualWindowRect();
        void ApplyRect(int borderSize, Rectangle rectangle);
        void PutInForground();
        void Minimize();
        void Restore();
    }
}