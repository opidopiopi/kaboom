using Kaboom.Abstraction;

namespace Kaboom.Domain.WindowTree.ArrangementAggregate
{
    public interface IBoundedTreeNode : ITreeNode<IBoundedTreeNode>, IHaveBounds, IEntity
    {

    }
}
