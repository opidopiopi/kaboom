namespace Kaboom.Abstraction
{
    public interface ITreeNode<T>
        where T : ITreeNode<T>
    {
        void InsertAsFirst(T child);
        void InsertAsLast(T child);

        void InsertBefore(T node, T reference);
        void InsertAfter(T node, T reference);

        void Remove(T node);

        bool IsLeaf();
    }
}