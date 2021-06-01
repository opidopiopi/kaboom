using Kaboom.Application.Actions;
using Kaboom.Application.Services;
using Kaboom.Domain.WindowTree;
using System;
using System.Linq;

namespace Plugins.Actions
{
    public class DebugTreePrint : IAction, IRenderService
    {
        private IArrangementRepository arrangementRepository;
        private int currentNestingLevel;

        public DebugTreePrint(IArrangementRepository arrangementRepository)
        {
            this.arrangementRepository = arrangementRepository;
        }

        public void Execute()
        {
            arrangementRepository
                .RootArrangements()
                .Select(rootID => arrangementRepository.Find(rootID))
                .ToList()
                .ForEach(root => ExecuteFromRoot(root));
        }

        public void ExecuteFromRoot(Arrangement rootArrangement)
        {
            Console.WriteLine();
            currentNestingLevel = 0;
            rootArrangement.Accept(this);
        }

        public void HighlightWindow(Window selectedWindow)
        {
        }

        public void Visit(Arrangement arrangement)
        {
            PrintNested(arrangement.ToString() + "\n");

            currentNestingLevel++;
            PrintNested("\n");

            arrangement.VisitAllChildren(this);

            currentNestingLevel--;
            PrintNested("\n");
        }

        public void Visit(Window window)
        {
            PrintNested(window.ToString() + "\n");
        }

        private void PrintNested(string nestedString)
        {
            for (int i = 0; i < currentNestingLevel; i++)
            {
                Console.Write(" |");
            }
            Console.Write(nestedString);
        }
    }
}
