using System.Collections.Generic;

namespace Kaboom.Abstraction
{
    public interface ITreeNode
    {
        void Remove(ITreeNode child);

        void InsertAsFirst(ITreeNode child);
        void InsertAsLast(ITreeNode child);

        ITreeNode FirstChild();
        ITreeNode LastChild();

        bool IsLeaf();
    }
}
