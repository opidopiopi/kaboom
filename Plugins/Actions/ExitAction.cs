using Kaboom.Application.Actions;

namespace Plugins.Actions
{
    public class ExitAction : IAction
    {
        private IShutdownHandler shutdownHandler;

        public ExitAction(IShutdownHandler shutdownHandler)
        {
            this.shutdownHandler = shutdownHandler;
        }

        public void Execute()
        {
            shutdownHandler.Shutdown();
        }
    }
}