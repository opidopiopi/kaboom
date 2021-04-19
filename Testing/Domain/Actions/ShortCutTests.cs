using Kaboom.Domain.ShortcutActions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kaboom.Testing.Domain.Actions
{
    [TestClass]
    public class ShortCutTests
    {

        [TestMethod]
        public void shortcut_all_keys_allowed()
        {
            new Shortcut(Modifier.WINDOWS, 'a');
            new Shortcut(Modifier.WINDOWS, 's');
            new Shortcut(Modifier.WINDOWS, 'k');
            new Shortcut(Modifier.WINDOWS, 'z');
            new Shortcut(Modifier.WINDOWS, 'A');
            new Shortcut(Modifier.WINDOWS, 'E');
            new Shortcut(Modifier.WINDOWS, 'I');
            new Shortcut(Modifier.WINDOWS, 'Z');
            new Shortcut(Modifier.WINDOWS, '0');
            new Shortcut(Modifier.WINDOWS, '4');
            new Shortcut(Modifier.WINDOWS, '7');
            new Shortcut(Modifier.WINDOWS, '9');
            new Shortcut(Modifier.WINDOWS, '+');
            new Shortcut(Modifier.WINDOWS, '?');
            new Shortcut(Modifier.WINDOWS, '%');
            new Shortcut(Modifier.WINDOWS, '§');
            new Shortcut(Modifier.WINDOWS, '"');
            new Shortcut(Modifier.WINDOWS, '~');
            new Shortcut(Modifier.WINDOWS, '-');
        }


        [TestMethod]
        public void shortcuts_are_the_same()
        {
            Assert.AreEqual(new Shortcut(Modifier.WINDOWS, 'a'), new Shortcut(Modifier.WINDOWS, 'a'));
            Assert.AreEqual(new Shortcut(Modifier.ALT, 'a'), new Shortcut(Modifier.ALT, 'a'));

            Assert.AreNotEqual(new Shortcut(Modifier.WINDOWS, 'a'), new Shortcut(Modifier.ALT, 'a'));
            Assert.AreNotEqual(new Shortcut(Modifier.ALT, 'b'), new Shortcut(Modifier.ALT, 'a'));

            Assert.AreNotEqual(new Shortcut(Modifier.WINDOWS, 'a'), new Shortcut(Modifier.WINDOWS, 'A'));
        }
    }
}
