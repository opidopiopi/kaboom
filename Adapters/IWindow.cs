using System;
using System.Drawing;

namespace Kaboom.Adapters
{
    public interface IWindow
    {
        IntPtr WindowHandle { get; }
        Rectangle PrefferedRectangle { get; set; }

        string WindowName();
        Rectangle GetActualWindowRect();
        void ApplyPreferredRect(int borderSize);
        void PutInForground();
    }
}