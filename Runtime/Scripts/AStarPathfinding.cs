/*
 * Copyright (c) 2026 Kokowolo. All Rights Reserved. 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: October 12, 2020
 * 
 * Additional Comments:
 *      The original version of this file can be found here: https://catlikecoding.com/unity/tutorials/hex-map/ within Catlike Coding's 
 *      tutorial series: Hex Map; this file has been updated it to better fit this project
 *
 *		File Line Length: ~140
 */


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kokowolo.Pathfinding
{
    public static class AStarPathfinding
    {
        /*██████████████████████████████████████████████████████████*/
        #region Events

        public static event Action OnStartSearch;
        // public static event Action OnStopSearch;
        public static event Action<OnNodeUpdatedEventArgs> OnNodeUpdated;
        public class OnNodeUpdatedEventArgs
        {
            public INode iNode;
        }

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Fields

        static NodePriorityQueue searchFrontier;
        
        static List<INode> searchedNodes = new List<INode>();

        static OnNodeUpdatedEventArgs args = new OnNodeUpdatedEventArgs();

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Properties

        /// <summary>
        /// The A* pathfinding rules of the current search.
        /// </summary>
        public static IPathfinding Pathfinding { get; set; }

        public static int SearchFrontierPhase { get; private set; }
        public static NodePath SearchPath = new NodePath();

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Functions

        public static bool TryAddNodeToPath(IPathfinder pathfinder, INode target, NodePath path, int maxDistance = int.MaxValue)
        {
            if (!path.IsValid) return false;

            // if (!path.End.Node.HasNeighbor(target)) return false;

            if (path.Penultimate == target)
            {
                path.RemoveAt(path.Length - 1);
                return path.IsValid && path.Length < maxDistance;
            }
            else if (!Pathfinding.CanCreatePathsWithRepeatNodes && path.Contains(target))
            {
                // FIXME: no option exists that allows for a path to go back and forth on two nodes because the Penultimate is removed
                return false; 
            }
            else if (pathfinder.IsValidMove(path.End, target))
            {
                // Is Distance/MoveCost Too Much?
                path.Start.PathFrom = path.End.PathFrom; // stores End.PathFrom within Start.PathFrom (currently unused)
                path.End.PathFrom = path.Penultimate;
                path.Add(target, pathfinder.GetMoveCost(path.End, target));
                path.End.PathFrom = path.Start.PathFrom; // returns original End.PathFrom value

                // Make Sure Path Hasn't Gotten Too Long
                return path.Length < maxDistance;
            }
            else
            {
                return false;
            }
        }

        // public static bool TryReduceOutsideMovementRangePath(IPathfinder pathfinder, NodePath path)
        // {
        //     bool hasReduced = false;
        //     while (path.IsValid && pathfinder.IsPathOutsideMovementRange(path))
        //     {
        //         path.RemoveAt(path.Length - 1);
        //         hasReduced = true;
        //     }
        //     return hasReduced;
        // }

        public static NodePath GetPath(IPathfinder pathfinder, INode start, INode end, int maxDistance = int.MaxValue)
        {
            OnStartSearch?.Invoke();

            SearchFrontierPhase += 2; // initialize new search frontier phase

            // initialize the search priority queue and searched nodes list
            if (searchFrontier == null) searchFrontier = new NodePriorityQueue();
            else searchFrontier.Clear();
            searchedNodes.Clear();

            // add the starting node to the queue
            UpdateNode(iNode: start, searchPhase: SearchFrontierPhase, distance: 0, pathFrom: null);
            searchFrontier.Enqueue(start);

            // as long as there is something in the queue, keep searching
            while (searchFrontier.Count > 0)
            {
                // pop current node 
                INode current = searchFrontier.Dequeue();
                UpdateNode(current, current.SearchPhase + 1, current.Distance, current.PathFrom);
                searchedNodes.Add(current);

                // check if we've found the target node
                if (current == end) 
                {
                    SetSearchPath(start, end);
                    return SearchPath;
                }

                foreach (INode neighbor in pathfinder.GetNeighborsFrom(current))
                {
                    // check if the neighbors are valid nodes to search
                    if (!IsValidMoveBetweenNodes(pathfinder, current, neighbor)) continue;
                    
                    // if they are valid, calculate distance and add them to the queue
                    int moveCost = pathfinder.GetMoveCost(current, neighbor);

                    // distance is calculated from move cost
                    int distance = current.Distance + moveCost;

                    // adding a new node that hasn't been updated
                    if (neighbor.SearchPhase < SearchFrontierPhase)
                    {
                        if (distance <= maxDistance)
                        {
                            UpdateNode(iNode: neighbor, searchPhase: SearchFrontierPhase, distance, pathFrom: current);
                            
                            // 3.3 Admissible Heuristic https://catlikecoding.com/unity/tutorials/hex-map/part-16/
                            if (end == null) neighbor.SearchHeuristic = 0; // searches everything
                            else neighbor.SearchHeuristic = pathfinder.GetHeuristicCost(neighbor, end);
                            
                            searchFrontier.Enqueue(neighbor);
                        }
                    }
                    else if (distance < neighbor.Distance) // adjusting node that's already in queue
                    {
                        int oldPriority = INode.GetSearchPriority(neighbor);
                        UpdateNode(iNode: neighbor, neighbor.SearchPhase, distance, pathFrom: current);
                        searchFrontier.Change(neighbor, oldPriority);
                    }
                }
            }
            SearchPath.Clear(); // TODO: break when found and clear this at the beginning
            return SearchPath;
        }

        public static List<INode> GetAllSearchedNodes(IPathfinder pathfinder, INode start, int maxDistance)
        {
            GetPath(pathfinder, start, null, maxDistance);
            return searchedNodes;
        }

        /// <summary>
        /// this is to specifically be called after GetAllSearchedNodes to avoid having to search again
        /// </summary>
        public static NodePath GetPreexistingPath(INode start, INode end)
        {
            SetSearchPath(start, end);
            return SearchPath;
        }

        static void UpdateNode(INode iNode, int searchPhase, int distance, INode pathFrom)
        {
            iNode.SearchPhase = searchPhase;
            iNode.Distance = distance;
            // node.MoveCost = pathFrom != null ? distance - pathFrom.Distance : distance;
            iNode.PathFrom = pathFrom;

            args.iNode = iNode;
            OnNodeUpdated?.Invoke(args);
        }

        static void SetSearchPath(INode start, INode end)
        {
            SearchPath.Clear();
            List<INode> path = new List<INode>();
            for (INode iNode = end; iNode != start; iNode = iNode.PathFrom) path.Add(iNode);
            path.Add(start);
            path.Reverse();
            SearchPath.Copy(path);
        }

        static bool IsValidMoveBetweenNodes(IPathfinder pathfinder, INode start, INode end)
        {
            // invalid if end is null or if the node is already out of the queue
            if (end.SearchPhase > SearchFrontierPhase) return false;

            return pathfinder.IsValidMove(start, end);
        }

        #endregion
        /*██████████████████████████████████████████████████████████*/
    }
}