/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 5/April/2015
 * Date Moddified :- 31/July/2015
 * </Copyright>
 */

using Microsoft.Xna.Framework;

namespace VaultTech.Algorithms.Pathfinding.ClearanceBasedAstar
{
    public class Node : IHeapItem<Node>
    {
        public Node Parent;
        public Vector2 Position;
        public bool Walkable = true;
        public bool InOpenList, InClosedList;
        public int gCost, hCost, Clearance;

        public int HeapIndex { get; set; }

        public int fCost
        {
            get { return hCost + gCost; }
            private set { }
        }

        public Node(Vector2 Position)
        {
            this.Position = Position;
        }

        public override bool Equals(object obj)
        {
            if (((Node)obj).Position == Position)
                return true;
            else
                return false;
        }

        public override int GetHashCode()
        {
            return Position.GetHashCode();
        }

        public int CompareTo(Node NodeToCompare)
        {
            int Compare = fCost.CompareTo(NodeToCompare.fCost);

            if (Compare == 0)
                Compare = hCost.CompareTo(NodeToCompare.hCost);

            return -Compare;
        }
    }
}