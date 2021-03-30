using Kaboom.Domain.WindowTree.Arrangements;
using Kaboom.Domain.WindowTree.Window;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kaboom.Application
{
    public class WindowService
    {
        public void MoveWindow(Arrangement rootArrangement, Window window, Direction direction)
        {
            var parent = rootArrangement.FindParentOf(window);
            var neighbour = parent.NeighbourOfChildInDirection(window, direction);

            //step one: try to move locally
            if (neighbour != null)
            {
                parent.MoveChild(window, direction);
                return;
            }
            else
            {
                parent.Remove(window);
                parent = rootArrangement.FindParentOf(parent);
            }

            //if we cant move locally, traverse up the tree until we find a node where we can move, or stop when we are at the root
            while (parent != null)
            {
                neighbour = parent.NeighbourOfChildInDirection(window, direction);

                if (neighbour != null)
                {
                    break;
                }

                parent = rootArrangement.FindParentOf(parent);
            }

            if(parent == null)
            {
                //move to other root
            }

            //parent.Insert next to arrsakjflöasdjflksdjflksdjaflök
        }
    }
}
