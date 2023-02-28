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
 *		File Line Length: 120
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
            public int searchFrontierPhase;
            public Node node;
        }

        #endregion
        /************************************************************/
        #region Fields

        private static int searchFrontierPhase;
        private static NodePriorityQueue searchFrontier;
        
        private static NodePath searchPath = new NodePath();

        private static AStarPathfindingEventArgs e = new AStarPathfindingEventArgs();

        #endregion
        /************************************************************/
        #region Properties

        #endregion
        /************************************************************/
        #region Functions

        public static bool TryAddNodeToPath(IPathfinding pathfinder, Node target, ref NodePath path)
        {
            if (!path.IsValid) return false;

            if (!path.End.HasNeighbor(target)) return false;

            if (path.Penultimate == target)
            {
                path.RemoveAt(path.Length - 1);
                return path.IsValid;
            }
            else if (path.Contains(target))
            {
                return false;
            }
            else if (pathfinder.IsValidMoveBetweenNodes(path.End, target))
            {
                path.Add(target);
                return !TryTrimPath(pathfinder, ref path);
            }
            else
            {
                return false;
            }
        }

        public static bool TryTrimPath(IPathfinding pathfinder, ref NodePath path)
        {
            bool trimmed = false;
            while (path.IsValid && pathfinder.IsPathTrimmable(path))
            {
                path.RemoveAt(path.Length - 1);
                trimmed = true;
            }
            return trimmed;
        }

        public static NodePath Search(IPathfinding pathfinder, Node start, Node end)
        {
            OnStartSearch?.Invoke(null, EventArgs.Empty);

            searchFrontierPhase += 2; // initialize new search frontier phase

            // initialize the search priority queue
            if (searchFrontier == null) searchFrontier = new NodePriorityQueue();
            else searchFrontier.Clear();

            // add the starting node to the queue
            SetNode(node: start, searchPhase: searchFrontierPhase, distance: 0, pathFrom: null);
            searchFrontier.Enqueue(start);

            // as long as there is something in the queue, keep searching
            while (searchFrontier.Count > 0)
            {
                // pop current node 
                Node current = searchFrontier.Dequeue();
                SetNode(current, current.SearchPhase + 1, current.Distance, current.PathFrom);

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
                    if (IsValidMoveBetweenNodes(pathfinder, start, neighbor))
                    {
                        // if they are valid, calculate distance and add them to the queue
                        int moveCost = pathfinder.GetMoveCostBetweenNodes(current, neighbor);

                        // distance is calculated from move cost
                        int distance = current.Distance + moveCost;

                        // adding a new node that hasn't been updated
                        if (neighbor.SearchPhase < searchFrontierPhase)
                        {
                            SetNode(node: neighbor, searchPhase: searchFrontierPhase, distance, pathFrom: current);

                            // because our lowest distance cost is 1, heuristic is just the DistanceTo()
                            neighbor.SearchHeuristic = 0;//neighbor.Coordinates.DistanceTo(end.Coordinates);
                            // FIXME: [LUTRO-265] Fix Search Heuristic - 0 was slapped on as a temporary fix

                            searchFrontier.Enqueue(neighbor);
                        }
                        else if (distance < neighbor.Distance) // adjusting node that's already in queue
                        {
                            int oldPriority = neighbor.SearchPriority;
                            SetNode(node: neighbor, neighbor.SearchPhase, distance, pathFrom: current);
                            searchFrontier.Change(neighbor, oldPriority);
                        }
                    }
                }
            }
            searchPath.Clear(); // TODO: break when found and clear this at the beginning
            return searchPath;
        }

        private static void SetNode(Node node, int searchPhase, int distance, Node pathFrom)
        {
            node.SearchPhase = searchPhase;
            node.Distance = distance;
            node.PathFrom = pathFrom;

            e.searchFrontierPhase = searchFrontierPhase;
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
            if (end.SearchPhase > searchFrontierPhase) return false;

            return pathfinder.IsValidMoveBetweenNodes(start, end);
        }

        #endregion
        /************************************************************/
    }
}