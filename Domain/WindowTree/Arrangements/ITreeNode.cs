using System.Collections.Generic;

namespace Kaboom.Domain.WindowTree.Arrangements
{
    public interface ITreeNode
    {
        TreeNodeID ID { get; }
        List<TreeNodeID> Children { get; }

        void InsertAsFirst(TreeNodeID child);
        void InsertAsLast(TreeNodeID child);
        void Remove(TreeNodeID child);

        TreeNodeID FirstChild();
        TreeNodeID LastChild();

        bool IsLeaf();
    }
}
