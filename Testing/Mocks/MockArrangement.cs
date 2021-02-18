using Kaboom.Abstract;
using Kaboom.Model;

namespace Testing.Mocks
{
    public class MockArrangement : WindowArrangement
    {
        public MockArrangement(Rectangle bounds) : base(bounds)
        {
        }

        protected override void UpdateBoundsOfChildren()
        {
            m_children.ForEach(child => ((IHaveBounds)child).Bounds = Bounds);
        }
    }
}
