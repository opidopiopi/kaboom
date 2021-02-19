using Kaboom.Abstract;
using Kaboom.Model;

namespace Testing.Mocks
{
    public class MockArrangement : WindowArrangement
    {
        protected override void UpdateBoundsOfChildren()
        {
            m_children.ForEach(child => ((IHaveBounds)child).Bounds = Bounds);
        }
    }
}
