/*
 * File Name: PathfindingNode.cs
 * Description: This script is for ...
 * 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: August 25, 2022
 * 
 * Additional Comments:
 *		File Line Length: 120
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using Kokowolo.Utilities;

namespace Kokowolo.Pathfinding
{
    [Serializable]
    public class Node
    {
        /************************************************************/
        #region Fields

        [NonSerialized] protected List<Node> neighbors;

        #endregion
        /************************************************************/
        #region Properties

        public object Instance { get; private set; }

        /// <summary>
        /// can this node be explored (visited)
        /// </summary>
        public bool IsExplorable { get; set; } = true;

        /// <summary>
        /// can this node be visited currently
        /// </summary>
        public bool IsVisitable { get; set; } = true;

        /// <summary>
        /// (G cost) property used for tracking a tile's distance from a source tile
        /// </summary>
        public int Distance { get; set; }

        /// <summary>
        /// (H Cost) reference to a tile's optimal/potential distance from a source tile; this value can be used to gauge 
        /// the possible distance this tile is from the source tile and will return the lowest potential distance cost
        /// </summary>
        public int SearchHeuristic { get; set; }

        /// <summary>
        /// (F Cost) reference to a tile's distance priority for when it should be evaluated in the search relative to other 
        /// tiles; this value is determined by a tile's current distance from the source tile and the search heuristic
        /// </summary>
        public int SearchPriority => Distance + SearchHeuristic;

        /// <summary>
        /// A reference tracker to a tile's previous neighbor that updated this tile's distance from a source tile; this 
        /// value can be recursively used to trace the path from a tile to the starting source tile
        /// </summary>
        public Node PathFrom { get; set; }

        /// <summary>
        /// A reference to a tile's adjacent neighbor in the linked list data structure of the PathfindingNodePriorityQueue 
        /// object; if this property is null, then the cell has no neighbor in the queue
        /// </summary>
        public Node NextWithSamePriority { get; set; }

        /// <summary>
        /// tracker of which phase of the search a tile is in; either not yet in the frontier [0], currently part of the 
        /// frontier [1], or behind the frontier [2]
        /// </summary>
        public int SearchPhase { get; set; }

        #endregion
        /************************************************************/
        #region Functions

        public Node(object instance)
        { 
            Instance = instance;
            neighbors = ListPool.Get<Node>();
        }

        // HACK: this is so PathfindingVisual can create duplicate nodes with independent Distance values; can this be cleaned up?
        public Node(Node node)
        {
            Instance = node.Instance;
            neighbors = ListPool.Get<Node>(node.neighbors);
            IsExplorable = node.IsExplorable;
            Distance = node.Distance;
            SearchHeuristic = node.SearchHeuristic;
            PathFrom = node.PathFrom;
            NextWithSamePriority = node.NextWithSamePriority;
            SearchPhase = node.SearchPhase;
        }

        ~Node()
        {
            ListPool.Add(neighbors);
        }

        public void ClearNeighbors()
        {
            neighbors.Clear();
        }

        public Node GetNeighbor(int index, bool ensureIsExplorable = true)
        {
            if (neighbors.Count <= index || neighbors[index] == null) return null;
            if (ensureIsExplorable && !neighbors[index].IsExplorable) return null;
            return neighbors[index];
        }

        public List<Node> GetNeighbors(bool ensureIsExplorable = true)
        {
            List<Node> nodes = ListPool.Get<Node>();
            foreach (Node neighbor in neighbors)
            {
                if (neighbor == null) continue;
                if (ensureIsExplorable && !neighbor.IsExplorable) continue;
                nodes.Add(neighbor);
            }
            return nodes;
        }

        public List<T> GetNeighbors<T>(bool ensureIsExplorable = true)
        {
            List<T> nodes = ListPool.Get<T>();
            foreach (Node neighbor in neighbors)
            {
                if (neighbor == null) continue;
                if (ensureIsExplorable && !neighbor.IsExplorable) continue;
                nodes.Add((T)neighbor.Instance);
            }
            return nodes;
        }

        public bool HasNeighbor(int index)
        {
            if (neighbors.Count <= index) return false;
            return neighbors[index] != null;
        }

        public bool HasNeighbor(Node node)
        {
            foreach (Node neighbor in neighbors)
            {
                if (node == neighbor) return true;
            }
            return false;
        }

        public void SetNeighbor(int index, Node node)
        {
            while (neighbors.Count <= index)
            {
                neighbors.Add(null);
            }
            neighbors[index] = node;
        }

        public void AddNeighbor(Node node)
        {
            for (int i = 0; i < neighbors.Count; i++)
            {
                if (neighbors[i] == null) 
                {
                    neighbors[i] = node;
                    return;
                }
            }
            neighbors.Add(node);
        }

        public override string ToString()
        {
            return $"({Distance}) {Instance}";
        }

        #endregion
        /************************************************************/
    }
}