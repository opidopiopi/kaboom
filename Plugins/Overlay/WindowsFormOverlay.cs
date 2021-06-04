using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Plugins.Overlay
{
    [ExcludeFromCodeCoverage]
    public class WindowsFormOverlay : Form, IOverlay
    {
        private List<IOverlayComponent> components = new List<IOverlayComponent>();

        public WindowsFormOverlay()
        {
            Text = WindowsRenderService.OVERLAY_NAME;
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.White;
            TransparencyKey = Color.White;
            TopMost = true;
            StartPosition = FormStartPosition.Manual;
            ShowInTaskbar = false;

            //let the form span over all screens
            Bounds = Screen.AllScreens.Select(screen => screen.Bounds).Aggregate((a, b) =>
                {
                    int minX = a.X < b.X ? a.X : b.X;
                    int minY = a.Y < b.Y ? a.Y : b.Y;
                    int maxX = a.X + a.Width > b.X + b.Width ? a.X + a.Width : b.X + b.Width;
                    int maxY = a.Y + a.Height > b.Y + b.Height ? a.Y + a.Height : b.Y + b.Height;

                    return new Rectangle(
                        minX,
                        minY,
                        maxX - minX,
                        maxY - minY
                    );
                });
            Paint += OverlayUpdate;
        }

        public void AddComponent(IOverlayComponent component)
        {
            components.Add(component);
        }

        public void RemoveComponent(IOverlayComponent component)
        {
            components.Remove(component);
        }

        public void ReRender()
        {
            Invoke(new MethodInvoker(delegate { Refresh(); }));
        }

        public void StartFormThread()
        {
            new Thread(() =>
            {
                Application.Run(this);
            }).Start();
        }

        private void OverlayUpdate(object sender, PaintEventArgs e)
        {
            var form = sender as WindowsFormOverlay;
            var graphics = e.Graphics;
            graphics.TranslateTransform(-form.Bounds.X, -form.Bounds.Y);

            components.ForEach(component => component.Render(graphics));
            graphics.Flush();
        }

        //make this form not show up in ALt+Tab
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                // turn on WS_EX_TOOLWINDOW style bit
                cp.ExStyle |= (int)Win32Wrapper.WindowStyles.WS_EX_TOOLWINDOW;
                return cp;
            }
        }
    }
}