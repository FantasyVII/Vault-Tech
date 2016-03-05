/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 5/April/2015
 * Date Moddified :- 24/September/2015
 * </Copyright>
 */

using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace VaultTech.Algorithms.Pathfinding.ClearanceBasedAstar
{
    public class Astar
    {
        MinHeap<Node> OpenList;
        public Node CurrentNode, StartingNode, TargetNode;

        public bool ReachedTarget;
        bool AllowDiagonalMovement;
        Node[,] GRID = new Node[(int)AstarGrid.GridSize.X, (int)AstarGrid.GridSize.Y];

        public Astar()
        {
        }

        /// <summary>
        /// This is the constructor for astar algorithem.
        /// </summary>
        /// <param name="StartingNode">The starting position in (Array coordinates) of the search path.</param>
        /// <param name="TargetNode">The target or destination position in (Array coordinates) where the search for the path will end at.</param>
        /// <param name="ObjectSize">The size of the object you want to find the path for. For exmaple if the size is set to one then the size will be equal to one square. If the size is set to two then the size will be equal to two squares on both x and y axies etc...</param>
        /// <param name="GridSize">Size of the grid that the A* algorithm will search in.</param>
        /// <param name="NoneWalkableNodes">Nodes that can not be walked through like walls.</param>
        /// <param name="AllowDiagonalMovement">If true, the A* algorithm will search for the path in a diagonal direction.</param>
        public Astar(Node StartingNode, Node TargetNode, bool AllowDiagonalMovement)
        {
            this.StartingNode = StartingNode;
            this.TargetNode = TargetNode;
            this.AllowDiagonalMovement = AllowDiagonalMovement;
        }

        List<Node> GetNeighbours()
        {
            List<Node> Neighbours = new List<Node>();

            for (int y = -1; y <= 1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    if (AllowDiagonalMovement)
                    {
                        if (x == 0 && y == 0)
                            continue;
                    }
                    else
                        if (x == 0 && y == 0 || x == -1 && y == -1 || x == 1 && y == -1 || x == 1 && y == 1 || x == -1 && y == 1)
                        continue;

                    //if the node in map bounds
                    if (CurrentNode.Position.X + x >= 0 && CurrentNode.Position.X + x < AstarGrid.GridSize.X &&
                        CurrentNode.Position.Y + y >= 0 && CurrentNode.Position.Y + y < AstarGrid.GridSize.Y)
                    {
                        Vector2 Position = new Vector2(CurrentNode.Position.X + x, CurrentNode.Position.Y + y);

                        if (GRID[(int)Position.X, (int)Position.Y] == null)
                        {
                            GRID[(int)Position.X, (int)Position.Y] = new Node(Position);
                            GRID[(int)Position.X, (int)Position.Y].Clearance = AstarGrid.Grid[(int)Position.X, (int)Position.Y].Clearance;
                            GRID[(int)Position.X, (int)Position.Y].Walkable = AstarGrid.Grid[(int)Position.X, (int)Position.Y].Walkable;
                        }

                        Neighbours.Add(GRID[(int)Position.X, (int)Position.Y]);
                    }
                }
            }

            return Neighbours;
        }

        int GetDistance(Node NodeA, Node NodeB)
        {
            Vector2 Distance = new Vector2(Math.Abs(NodeA.Position.X - NodeB.Position.X), Math.Abs(NodeA.Position.Y - NodeB.Position.Y));
            /*
            if (Distance.X > Distance.Y)
                return (int)(14 * Distance.Y + 10 * (Distance.X - Distance.Y));
            else
                return (int)(14 * Distance.X + 10 * (Distance.Y - Distance.X));*/

            return (int)(Distance.X + Distance.Y);
        }

        public List<Vector2> GetFinalPath()
        {
            if (ReachedTarget)
            {
                List<Vector2> FinalPath = new List<Vector2>();

                while (true)
                {
                    if (CurrentNode.Parent != null)
                    {
                        FinalPath.Add(CurrentNode.Position);
                        CurrentNode = CurrentNode.Parent;
                    }
                    else
                        break;
                }
                FinalPath.Add(StartingNode.Position);
                FinalPath.Reverse();

                return FinalPath;
            }
            else
                return new List<Vector2>();
        }

        void Search()
        {
            List<Node> Neighbours = GetNeighbours();

            foreach (Node Neighbour in Neighbours)
            {
                if (Neighbour.InClosedList || !Neighbour.Walkable || Neighbour.Clearance < AstarGrid.ObjectSize)
                    continue;

                int NewMovementCost = CurrentNode.gCost + GetDistance(CurrentNode, Neighbour);

                if (!Neighbour.InOpenList || NewMovementCost < Neighbour.gCost)
                {
                    Neighbour.gCost = NewMovementCost;
                    Neighbour.hCost = GetDistance(CurrentNode, TargetNode);
                    Neighbour.Parent = CurrentNode;

                    if (!Neighbour.InOpenList)
                    {
                        Neighbour.InOpenList = true;
                        OpenList.Push(Neighbour);
                    }
                    else
                        OpenList.UpdateItem(Neighbour);
                }
            }
        }

        internal void FindPath()
        {
            OpenList = new MinHeap<Node>((int)(AstarGrid.GridSize.X * AstarGrid.GridSize.Y));

            CurrentNode = StartingNode;

            GRID[(int)CurrentNode.Position.X, (int)CurrentNode.Position.Y] = new Node(new Vector2(CurrentNode.Position.X, CurrentNode.Position.Y));
            GRID[(int)CurrentNode.Position.X, (int)CurrentNode.Position.Y].Clearance = AstarGrid.Grid[(int)CurrentNode.Position.X, (int)CurrentNode.Position.Y].Clearance;
            GRID[(int)CurrentNode.Position.X, (int)CurrentNode.Position.Y].Walkable = AstarGrid.Grid[(int)CurrentNode.Position.X, (int)CurrentNode.Position.Y].Walkable;

            CurrentNode.hCost = GetDistance(CurrentNode, TargetNode);

            OpenList.Push(CurrentNode);

            while (OpenList.Count > 0)
            {
                CurrentNode = OpenList.Pop();

                if (CurrentNode.Equals(TargetNode))
                {
                    ReachedTarget = true;
                    break;
                }

                GRID[(int)CurrentNode.Position.X, (int)CurrentNode.Position.Y].InClosedList = true;
                Search();
            }
        }
    }
}