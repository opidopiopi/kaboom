namespace Kaboom.Domain.WindowTree.Arrangements
{
    public interface ITreeNodeRepository
    {
        void AddTreeNode(ITreeNode treeNode);
        void RemoveTreeNode(TreeNodeID treeNode);

        ITreeNode Find(TreeNodeID nodeID);
    }
}
