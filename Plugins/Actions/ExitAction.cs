using Kaboom.Application.Actions;

namespace Plugins.Actions
{
    public class ExitAction : IAction
    {
        public void Execute()
        {
            System.Environment.Exit(1);
        }
    }
}