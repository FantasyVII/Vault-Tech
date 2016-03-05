/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 22/September/2015
 * Date Moddified :- 24/September/2015
 * </Copyright>
 */

using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace VaultTech.Algorithms.Pathfinding.ClearanceBasedAstar
{
    public static class AstarGrid
    {
        internal static Node[,] Grid;
        internal static Vector2 GridSize;
        static HashSet<Vector2> NoneWalkableHashsetNodes;
        static internal int ObjectSize;

        /// <summary>
        /// This is the constructor for astar algorithem.
        /// </summary>
        /// <param name="ObjectSize">The size of the object you want to find the path for. For exmaple if the size is set to one then the size will be equal to one square. If the size is set to two then the size will be equal to two squares on both x and y axies etc...</param>
        /// <param name="GridSize">Size of the grid that the A* algorithm will search in.</param>
        /// <param name="NoneWalkableNodes">Nodes that can not be walked through like walls.</param>
        internal static void PrepareGrid(int ObjectSize, int MaxObjectSize, Vector2 GridSize, List<Node> NoneWalkableNodes)
        {
            AstarGrid.ObjectSize = ObjectSize;
            AstarGrid.GridSize = GridSize;

            NoneWalkableHashsetNodes = new HashSet<Vector2>();

            Grid = new Node[(int)GridSize.X, (int)GridSize.Y];

            for (int y = 0; y < GridSize.Y; y++)
                for (int x = 0; x < GridSize.X; x++)
                    Grid[x, y] = new Node(new Vector2(x, y));

            for (int i = 0; i < NoneWalkableNodes.Count; i++)
                Grid[(int)NoneWalkableNodes[i].Position.X, (int)NoneWalkableNodes[i].Position.Y].Walkable = false;

            for (int i = 0; i < NoneWalkableNodes.Count; i++)
                NoneWalkableHashsetNodes.Add(NoneWalkableNodes[i].Position);

            CalculateClearanceMetric(MaxObjectSize);
        }

        static void CalculateClearanceMetric(int MaxObjectSize)
        {
            for (int y = 0; y < GridSize.Y; y++)
            {
                for (int x = 0; x < GridSize.X; x++)
                {
                    bool Escape = false;
                    int Clearance = 0;
                    Rectangle NodeRectangle = new Rectangle(x, y, 1, 1);

                    while (!Escape)
                    {
                        if (NodeRectangle.X + Clearance >= GridSize.X || NodeRectangle.Y + Clearance >= GridSize.Y)
                            break;

                        if (Clearance < MaxObjectSize)
                        {
                            Clearance += 1;
                            NodeRectangle = new Rectangle(x, y, Clearance, Clearance);

                            for (int i = 0; i < Clearance; i++)
                            {
                                if (NoneWalkableHashsetNodes.Contains(new Vector2(NodeRectangle.X + i, NodeRectangle.Y + Clearance - 1)) ||
                                    NoneWalkableHashsetNodes.Contains(new Vector2(NodeRectangle.X + Clearance - 1, NodeRectangle.Y + i)))
                                {
                                    Clearance -= 1;
                                    Escape = true;
                                    break;
                                }
                            }
                        }
                        else
                            Escape = true;
                    }

                    Grid[x, y].Clearance = Clearance;
                }
            }
        }
    }
}