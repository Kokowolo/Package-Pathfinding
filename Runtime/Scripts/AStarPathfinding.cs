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


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

namespace Kokowolo.Pathfinding
{
    public static class AStarPathfinding
    {
        /*██████████████████████████████████████████████████████████*/
        #region Events

        public static event Action OnStartSearch;
        public static event Action<AStarPathfindingEventArgs> OnSetNode;

        public class AStarPathfindingEventArgs
        {
            public INode iNode;
        }

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Fields

        static INodePriorityQueue searchFrontier;
        
        static INodePath searchPath = new INodePath();
        static List<INode> searchedNodes = new List<INode>();

        static AStarPathfindingEventArgs args = new AStarPathfindingEventArgs();

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Properties

        public static int SearchFrontierPhase { get; private set; }

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Functions

        public static bool TryAddNodeToPath(IPathfinding pathfinder, INode target, INodePath path)
        {
            if (!path.IsValid) return false;

            if (!path.End.Node.HasNeighbor(target)) return false;

            if (path.Penultimate == target)
            {
                path.RemoveAt(path.Length - 1);
                return path.IsValid && !pathfinder.IsPathOutsideMovementRange(path);
            }
            else if (!pathfinder.CanCreatePathsWithRepeatNodes && path.Contains(target))
            {
                // FIXME: no option exists that allows for a path to go back and forth on two nodes because the Penultimate is removed
                return false; 
            }
            else if (pathfinder.IsValidMoveBetweenNodes(path.End, target))
            {
                // Is Move Cost Too Much?
                path.Start.Node.PathFrom = path.End.Node.PathFrom; // stores End.PathFrom within Start.PathFrom (currently unused)
                path.End.Node.PathFrom = path.Penultimate;
                path.Add(target, pathfinder.GetMoveCostBetweenNodes(path.End, target));
                path.End.Node.PathFrom = path.Start.Node.PathFrom; // returns original End.PathFrom value

                // Make Sure Path Hasn't Gotten Too Long
                return !pathfinder.IsPathOutsideMovementRange(path);
            }
            else
            {
                return false;
            }
        }

        public static bool TryReduceOutsideMovementRangePath(IPathfinding pathfinder, INodePath path)
        {
            bool hasReduced = false;
            while (path.IsValid && pathfinder.IsPathOutsideMovementRange(path))
            {
                path.RemoveAt(path.Length - 1);
                hasReduced = true;
            }
            return hasReduced;
        }

        public static INodePath GetPath(IPathfinding pathfinder, INode start, INode end, int maxDistance = int.MaxValue)
        {
            OnStartSearch?.Invoke();

            SearchFrontierPhase += 2; // initialize new search frontier phase

            // initialize the search priority queue and searched nodes list
            if (searchFrontier == null) searchFrontier = new INodePriorityQueue();
            else searchFrontier.Clear();
            searchedNodes.Clear();

            // add the starting node to the queue
            SetNode(iNode: start, searchPhase: SearchFrontierPhase, distance: 0, pathFrom: null);
            searchFrontier.Enqueue(start);

            // as long as there is something in the queue, keep searching
            while (searchFrontier.Count > 0)
            {
                // pop current node 
                INode current = searchFrontier.Dequeue();
                SetNode(current, current.Node.SearchPhase + 1, current.Node.Distance, current.Node.PathFrom);
                searchedNodes.Add(current);

                // check if we've found the target node
                if (current == end) 
                {
                    SetSearchPath(start, end);
                    return searchPath;
                }

                List<INode> neighbors = pathfinder.GetNeighborsFromNode(current);

                foreach (INode neighbor in neighbors)
                {
                    // check if the neighbors are valid nodes to search
                    if (!IsValidMoveBetweenNodes(pathfinder, current, neighbor)) continue;
                    
                    // if they are valid, calculate distance and add them to the queue
                    int moveCost = pathfinder.GetMoveCostBetweenNodes(current, neighbor);

                    // distance is calculated from move cost
                    int distance = current.Node.Distance + moveCost;

                    // adding a new node that hasn't been updated
                    if (neighbor.Node.SearchPhase < SearchFrontierPhase)
                    {
                        if (distance <= maxDistance)
                        {
                            SetNode(iNode: neighbor, searchPhase: SearchFrontierPhase, distance, pathFrom: current);
                            
                            // 3.3 Admissible Heuristic https://catlikecoding.com/unity/tutorials/hex-map/part-16/
                            if (end == null) neighbor.Node.SearchHeuristic = 0; // searches everything
                            else neighbor.Node.SearchHeuristic = pathfinder.GetHeuristicCostBetweenNodes(neighbor, end);
                            
                            searchFrontier.Enqueue(neighbor);
                        }
                    }
                    else if (distance < neighbor.Node.Distance) // adjusting node that's already in queue
                    {
                        int oldPriority = neighbor.Node.SearchPriority;
                        SetNode(iNode: neighbor, neighbor.Node.SearchPhase, distance, pathFrom: current);
                        searchFrontier.Change(neighbor, oldPriority);
                    }
                }
            }
            searchPath.Clear(); // TODO: break when found and clear this at the beginning
            return searchPath;
        }

        public static List<INode> GetAllSearchedNodes(IPathfinding pathfinder, INode start, int maxDistance)
        {
            GetPath(pathfinder, start, null, maxDistance);
            return searchedNodes;
        }

        /// <summary>
        /// this is to specifically be called after GetAllSearchedNodes to avoid having to search again
        /// </summary>
        public static INodePath GetPreexistingPath(INode start, INode end)
        {
            SetSearchPath(start, end);
            return searchPath;
        }

        static void SetNode(INode iNode, int searchPhase, int distance, INode pathFrom)
        {
            iNode.Node.SearchPhase = searchPhase;
            iNode.Node.Distance = distance;
            // node.MoveCost = pathFrom != null ? distance - pathFrom.Distance : distance;
            iNode.Node.PathFrom = pathFrom;

            args.iNode = iNode;
            OnSetNode?.Invoke(args);
        }

        static void SetSearchPath(INode start, INode end)
        {
            searchPath.Clear();
            List<INode> path = new List<INode>();
            for (INode iNode = end; iNode != start; iNode = iNode.Node.PathFrom) path.Add(iNode);
            path.Add(start);
            path.Reverse();
            searchPath.Copy(path);
        }

        static bool IsValidMoveBetweenNodes(IPathfinding pathfinder, INode start, INode end)
        {
            // invalid if end is null or if the node is already out of the queue
            if (end.Node.SearchPhase > SearchFrontierPhase) return false;

            return pathfinder.IsValidMoveBetweenNodes(start, end);
        }

        #endregion
        /*██████████████████████████████████████████████████████████*/
    }
}