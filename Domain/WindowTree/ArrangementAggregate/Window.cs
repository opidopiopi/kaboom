using Kaboom.Abstraction;

namespace Kaboom.Domain.WindowTree.ArrangementAggregate
{
    public class Window : BoundedTreeLeaf
    {
        public string Title { get; }
        public bool Visible { get; set; }

        public Window(Rectangle initialBounds, string title)
        {
            Bounds = initialBounds;
            Title = title;
            Visible = true;
        }
    }
}
