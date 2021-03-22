using Kaboom.Domain.WindowTree.Arrangements;
using System.Collections.Generic;

namespace Kaboom.Testing.Mocks
{
    public class MockTreeNodeRepository : ITreeNodeRepository
    {
        private List<ITreeNode> m_treeNodes = new List<ITreeNode>();
        public void AddTreeNode(ITreeNode treeNode)
        {
            m_treeNodes.Add(treeNode);
        }

        public ITreeNode Find(TreeNodeID nodeID)
        {
            return m_treeNodes.Find(node => node.ID == nodeID);
        }

        public void RemoveTreeNode(TreeNodeID treeNode)
        {
            m_treeNodes.RemoveAll(node => node.ID == treeNode);
        }
    }
}
