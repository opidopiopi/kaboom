using Kaboom.Abstraction;
using Kaboom.Domain.WindowTree.ValueObjects;

namespace Kaboom.Domain.WindowTree.Helpers
{
    public interface IBoundedTreeNode : ITreeNode<IBoundedTreeNode>, IEntity
    {
        Bounds Bounds { get; set; }
        bool Visible { get; set; }

        void Accept(IVisitor visitor);
    }
}
