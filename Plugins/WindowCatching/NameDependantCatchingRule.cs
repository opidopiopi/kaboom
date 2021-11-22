using Kaboom.Adapters;
using System.Text.RegularExpressions;

namespace Plugins.WindowCatching
{
    public class NameDependantCatchingRule : DefaultCatchingRule
    {
        private Regex includeRegex;
        private Regex excludeRegex;

        public NameDependantCatchingRule(string includeRegex, string excludeRegex)
        {
            this.includeRegex = new Regex(includeRegex);
            this.excludeRegex = new Regex(excludeRegex);
        }

        public override bool DoWeWantToCatchThisWindow(IWindow window)
        {
            string name = window.WindowName();

            if(excludeRegex.IsMatch(name))
            {
                return false; //exclude means exclude
            }

            bool result = base.DoWeWantToCatchThisWindow(window);
            result |= includeRegex.IsMatch(name); //include means include

            return result;
        }
    }
}
