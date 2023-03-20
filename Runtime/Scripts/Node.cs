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

        [NonSerialized] private List<Node> neighbors;

        #endregion
        /************************************************************/
        #region Properties

        public object Object { get; private set; }

        /// <summary>
        /// can this node be explored (visited)
        /// </summary>
        public bool IsExplorable { get; set; } = true;

        /// <summary>
        /// can this node be visited currently
        /// </summary>
        public bool IsWalkable { get; set; } = true;

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
        /// frontier [1], or behind the frontier 2
        /// </summary>
        public int SearchPhase { get; set; }

        #endregion
        /************************************************************/
        #region Functions

        public Node(object obj)
        {
            Object = obj;   
            neighbors = ListPool.Get<Node>();
        }

        // HACK: this is so PathfindingVisual can create duplicate nodes with independent Distance values; can this be cleaned up?
        public Node(Node node)
        {
            Object = node.Object;
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
            ListPool.Release(neighbors);
        }

        public void ClearNeighbors()
        {
            neighbors.Clear();
        }

        public List<Node> GetNeighbors(bool includeOnlyExplorable = true)
        {
            List<Node> nodes = ListPool.Get<Node>();
            foreach (Node neighbor in neighbors)
            {
                // if (neighbor == null) continue;
                if (includeOnlyExplorable && !neighbor.IsExplorable) continue;
                nodes.Add(neighbor);
            }
            return nodes;
        }

        public bool HasNeighbor(Node node)
        {
            foreach (Node neighbor in neighbors)
            {
                if (node == neighbor) return true;
            }
            return false;
        }

        public void AddNeighbor(Node node)
        {
            if (neighbors.Contains(node)) return;
            neighbors.Add(node);
        }

        public override string ToString()
        {
            return $"({Distance}) {Object}";
        }

        #endregion
        /************************************************************/
    }
}