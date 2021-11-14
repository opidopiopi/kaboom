using Kaboom.Application.Actions;

namespace Kaboom.Testing.Mocks
{
    public class MockAction : IAction
    {
        public int TriggerCount = 0;

        public void Execute()
        {
            TriggerCount++;
        }

        public override string ToString()
        {
            return $"(MockAction: TriggerCount:{TriggerCount})";
        }
    }
}
