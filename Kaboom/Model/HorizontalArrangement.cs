using Kaboom.Abstract;
using System.Collections.Generic;
using System.Linq;

namespace Kaboom.Model
{
    public class HorizontalArrangement : WindowArrangement
    {
        protected override void UpdateBoundsOfChildren()
        {
            if (m_children.Count == 0) return;

            List<IHaveBounds> children = m_children.Cast<IHaveBounds>().ToList();   //I don't like

            int numberOfChildren = children.Count();
            int widthPerChild = Bounds.Width / numberOfChildren;

            for(int i = 0; i < numberOfChildren; i++)
            {
                children[i].Bounds = new Rectangle(Bounds.X + i * widthPerChild, Bounds.Y, widthPerChild, Bounds.Height);
            }
        }
    }
}
