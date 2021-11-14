using Kaboom.Domain.WindowTree.Helpers;
using Kaboom.Domain.WindowTree.ValueObjects;
using System.Collections.Generic;

namespace Kaboom.Domain.WindowTree
{
    public class Window : BoundedTreeLeaf
    {
        public string Title { get; }

        public Window(Bounds initialBounds, string title)
        {
            Bounds = initialBounds;
            Title = title;
            Visible = true;
        }

        public override string ToString()
        {
            return $"(Window Title: {Title}, ID: {ID}, Visible: {Visible})";
        }

        public override bool Equals(object obj)
        {
            return obj is Window window &&
                   EqualityComparer<EntityID>.Default.Equals(ID, window.ID);
        }

        public override int GetHashCode()
        {
            return 1213502048 + EqualityComparer<EntityID>.Default.GetHashCode(ID);
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
