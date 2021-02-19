using System.Collections.Generic;

namespace Kaboom.Abstract
{
    public interface ITreeNode
    {
        void Insert(ITreeNode child);
        bool RemoveAndReturnSuccess(ITreeNode child);

        void SetParent(ITreeNode parent);
        ITreeNode GetParent();

        List<ITreeNode> Children();

        bool IsLeaf();
    }
}
