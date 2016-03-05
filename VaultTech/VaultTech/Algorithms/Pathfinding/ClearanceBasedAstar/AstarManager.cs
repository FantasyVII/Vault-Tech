/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 3/December/2014
 * Date Moddified :- 24/September/2015
 * </Copyright>
 */

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

using Microsoft.Xna.Framework;

namespace VaultTech.Algorithms.Pathfinding.ClearanceBasedAstar
{
    /// <summary>
    /// This class is responsible for creating a thread from a ThreadPool 
    /// so the A* algorithm can run on and produce It's results.
    /// </summary>
    public static class AstarManager
    {
        /// <summary>
        /// A ConcurrentQueue where every AstarThreadWorker will store its results in.
        /// </summary>
        internal static ConcurrentDictionary<int, Astar> AstarThreadWorkerResults = new ConcurrentDictionary<int, Astar>();
        
        public static void PrepareAstarMap(int ObjectSize, int MaxObjectSize, Vector2 GridSize, List<Node> NoneWalkableNodes)
        {
            AstarGrid.PrepareGrid(ObjectSize, MaxObjectSize, GridSize, NoneWalkableNodes);
        }

        /// <summary>
        /// This function will add a new thread worker for A* algorithm to run on.
        /// </summary>
        /// <param name="aStar">This is the A* class.</param>
        /// <param name="WorkerIDNumber">ID number for this worker thread so you can get the results back.</param>
        public static void TryAddNewThreadWorker(Astar aStar, int WorkerIDNumber)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(delegate
            {
                aStar.FindPath();
                AstarThreadWorkerResults.TryAdd(WorkerIDNumber, aStar);
            }));
        }

        /// <summary>
        /// This function will try to return the results that was sumbited trhough the AddNewThreadWorkder function using the WorkerIDNumber. If it can't find anything it will return null.
        /// </summary>
        /// <param name="WorkerIDNumber">ID number of the worker thread you want to retrieve the results from.</param>
        /// <returns>Return A* call results</returns>
        public static Astar TryRemoveAndReturnAstarResult(int WorkerIDNumber, bool WaitForThread)
        {
            Astar AstarResult;

            while (true)
            {
                AstarThreadWorkerResults.TryRemove(WorkerIDNumber, out AstarResult);

                if (AstarResult != null)
                    return AstarResult;

                if (!WaitForThread)
                    return new Astar();
            }
        }
    }
}