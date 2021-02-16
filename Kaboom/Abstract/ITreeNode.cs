using System.Collections.Generic;

namespace Kaboom.Abstract
{
    public interface ITreeNode
    {
        void Insert(ITreeNode child);
        void Remove(ITreeNode child);

        void SetParent(ITreeNode parent);
        ITreeNode GetParent();
    }
}
