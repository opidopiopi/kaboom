using Kaboom.Abstract;
using System.Collections.Generic;

namespace Kaboom.Model
{
    public class Workspace : ITreeNode
    {
        private List<Screen> m_screens = new List<Screen>();

        public Workspace(IProvideScreens screenProvider)
        {
            screenProvider.GetScreens().ForEach(Insert);
        }

        public void Insert(Screen screen)
        {
            m_screens.Add(screen);
            screen.Insert(new HorizontalArrangement(new Rectangle(0, 0, 1, 1)));
            screen.SetParent(this);
        }

        public void Insert(ITreeNode child)
        {
            throw new System.NotImplementedException();
        }

        public void Remove(ITreeNode child)
        {
            throw new System.NotImplementedException();
        }

        public void SetParent(ITreeNode parent)
        {
            throw new System.NotImplementedException();
        }

        public ITreeNode GetParent()
        {
            throw new System.NotImplementedException();
        }
    }
}
