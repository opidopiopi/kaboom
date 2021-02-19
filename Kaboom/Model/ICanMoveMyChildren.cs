using Kaboom.Abstract;

namespace Kaboom.Model
{
    public interface ICanMoveMyChildren
    {
        public void MoveChildUp(ITreeNode child, ITreeNode caller);
        public void MoveChildDown(ITreeNode child, ITreeNode caller);
        public void MoveChildLeft(ITreeNode child, ITreeNode caller);
        public void MoveChildRight(ITreeNode child, ITreeNode caller);
    }
}
