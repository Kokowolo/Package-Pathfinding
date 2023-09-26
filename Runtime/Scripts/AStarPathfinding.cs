/**
 * File Name: Pathfinding.cs
 * Description: 
 * 
 * Authors: Will Lacey
 * Date Created: October 12, 2020
 * 
 * Additional Comments:
 *      The original version of this file can be found here:
 *      https://catlikecoding.com/unity/tutorials/hex-map/ within Catlike Coding's tutorial series:
 *      Hex Map; this file has been updated it to better fit this project
 *
 *      File Line Length: 120
 **/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using Kokowolo.Utilities;

namespace Kokowolo.Pathfinding
{
    public static class AStarPathfinding
    {
        /************************************************************/
        #region Events

        public static event EventHandler OnStartSearch;
        public static event EventHandler<AStarPathfindingEventArgs> OnSetNode;

        public class AStarPathfindingEventArgs
        {
            public Node node;
        }

        #endregion
        /************************************************************/
        #region Fields

        private static NodePriorityQueue searchFrontier;
        
        private static NodePath searchPath = new NodePath();
        private static List<Node> searchedNodes = new List<Node>();

        private static AStarPathfindingEventArgs e = new AStarPathfindingEventArgs();

        #endregion
        /************************************************************/
        #region Properties

        public static int SearchFrontierPhase { get; private set; }

        #endregion
        /************************************************************/
        #region Functions

        public static bool TryAddNodeToPath(IPathfinding pathfinder, Node target, NodePath path)
        {
            if (!path.IsValid) return false;

            if (!path.End.HasNeighbor(target)) return false;

            if (path.Penultimate == target)
            {
                path.RemoveAt(path.Length - 1);
                return path.IsValid && !pathfinder.IsPathOutsideMovementRange(path);
            }
            else if (!pathfinder.CanCreatePathsWithRepeatNodes && path.Contains(target))
            {
                // FIXME: no option exists that allows for a unit to go back and forth on two nodes because the Penultimate is removed
                return false; 
            }
            else if (pathfinder.IsValidMoveBetweenNodes(path.End, target))
            {
                // Is Move Cost Too Much?
                path.Start.PathFrom = path.End.PathFrom; // stores End.PathFrom within Start.PathFrom (currently unused)
                path.End.PathFrom = path.Penultimate;
                path.Add(target, pathfinder.GetMoveCostBetweenNodes(path.End, target));
                path.End.PathFrom = path.Start.PathFrom; // returns original End.PathFrom value

                // Make Sure Path Hasn't Gotten Too Long
                return !pathfinder.IsPathOutsideMovementRange(path);
            }
            else
            {
                return false;
            }
        }

        public static bool TryReduceOutsideMovementRangePath(IPathfinding pathfinder, NodePath path)
        {
            bool hasReduced = false;
            while (path.IsValid && pathfinder.IsPathOutsideMovementRange(path))
            {
                path.RemoveAt(path.Length - 1);
                hasReduced = true;
            }
            return hasReduced;
        }

        public static NodePath GetPath(IPathfinding pathfinder, Node start, Node end, int maxDistance = int.MaxValue)
        {
            OnStartSearch?.Invoke(null, EventArgs.Empty);

            SearchFrontierPhase += 2; // initialize new search frontier phase

            // initialize the search priority queue and searched nodes list
            if (searchFrontier == null) searchFrontier = new NodePriorityQueue();
            else searchFrontier.Clear();
            searchedNodes.Clear();

            // add the starting node to the queue
            SetNode(node: start, searchPhase: SearchFrontierPhase, distance: 0, pathFrom: null);
            searchFrontier.Enqueue(start);

            // as long as there is something in the queue, keep searching
            while (searchFrontier.Count > 0)
            {
                // pop current node 
                Node current = searchFrontier.Dequeue();
                SetNode(current, current.SearchPhase + 1, current.Distance, current.PathFrom);
                searchedNodes.Add(current);

                // check if we've found the target node
                if (current == end) 
                {
                    SetSearchPath(start, end);
                    return searchPath;
                }

                List<Node> neighbors = pathfinder.GetNeighborsFromNode(current);

                foreach (Node neighbor in neighbors)
                {
                    // check if the neighbors are valid nodes to search
                    if (!IsValidMoveBetweenNodes(pathfinder, current, neighbor)) continue;
                    
                    // if they are valid, calculate distance and add them to the queue
                    int moveCost = pathfinder.GetMoveCostBetweenNodes(current, neighbor);

                    // distance is calculated from move cost
                    int distance = current.Distance + moveCost;

                    // adding a new node that hasn't been updated
                    if (neighbor.SearchPhase < SearchFrontierPhase)
                    {
                        if (distance <= maxDistance)
                        {
                            SetNode(node: neighbor, searchPhase: SearchFrontierPhase, distance, pathFrom: current);
                            
                            // 3.3 Admissible Heuristic https://catlikecoding.com/unity/tutorials/hex-map/part-16/
                            if (end == null) neighbor.SearchHeuristic = 0; // searches everything
                            else neighbor.SearchHeuristic = pathfinder.GetHeuristicCostBetweenNodes(neighbor, end);
                            
                            searchFrontier.Enqueue(neighbor);
                        }
                    }
                    else if (distance < neighbor.Distance) // adjusting node that's already in queue
                    {
                        int oldPriority = neighbor.SearchPriority;
                        SetNode(node: neighbor, neighbor.SearchPhase, distance, pathFrom: current);
                        searchFrontier.Change(neighbor, oldPriority);
                    }
                }
            }
            searchPath.Clear(); // TODO: break when found and clear this at the beginning
            return searchPath;
        }

        public static List<Node> GetAllSearchedNodes(IPathfinding pathfinder, Node start, int maxDistance)
        {
            GetPath(pathfinder, start, null, maxDistance);
            return searchedNodes;
        }

        /// <summary>
        /// this is to specifically be called after GetAllSearchedNodes to avoid having to search again
        /// </summary>
        public static NodePath GetPreexistingPath(Node start, Node end)
        {
            SetSearchPath(start, end);
            return searchPath;
        }

        private static void SetNode(Node node, int searchPhase, int distance, Node pathFrom)
        {
            node.SearchPhase = searchPhase;
            node.Distance = distance;
            // node.MoveCost = pathFrom != null ? distance - pathFrom.Distance : distance;
            node.PathFrom = pathFrom;

            e.node = node;
            OnSetNode?.Invoke(null, e);
        }

        private static void SetSearchPath(Node start, Node end)
        {
            searchPath.Clear();
            List<Node> path = new List<Node>();
            for (Node node = end; node != start; node = node.PathFrom) path.Add(node);
            path.Add(start);
            path.Reverse();
            searchPath.Copy(path);
        }

        private static bool IsValidMoveBetweenNodes(IPathfinding pathfinder, Node start, Node end)
        {
            // invalid if end is null or if the node is already out of the queue
            if (end.SearchPhase > SearchFrontierPhase) return false;

            return pathfinder.IsValidMoveBetweenNodes(start, end);
        }

        #endregion
        /************************************************************/
    }
}