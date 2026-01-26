/*
 * Copyright (c) 2026 Kokowolo. All Rights Reserved. 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: December 19, 2024
 * 
 * Additional Comments:
 *		File Line Length: ~140
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kokowolo.Pathfinding
{
    public interface INode
    {
        /// <summary>
        /// Can this node be explored (this is essentially visited if no fog of war)?
        /// </summary>
        public bool IsExplorable { get; set; }

        /// <summary>
        /// Can this node be visited currently?
        /// </summary>
        public bool IsVisitable { get; set; }

        /// <summary>
        /// (G cost) Property used for tracking a tile's distance from a source tile.
        /// </summary>
        public int Distance { get; set; }

        /// <summary>
        /// (H Cost) Reference to a tile's optimal/potential distance from a source tile; this value can be used to gauge the possible 
        /// distance this tile is from the source tile and will return the lowest potential distance cost.
        /// </summary>
        public int SearchHeuristic { get; set; }

        /// <summary>
        /// (F Cost) Reference to a tile's distance priority for when it should be evaluated in the search relative to other tiles; this 
        /// value is determined by a tile's current distance from the source tile and the search heuristic.
        /// </summary>
        public static int GetSearchPriority(INode iNode) => iNode.Distance + iNode.SearchHeuristic;

        /// <summary>
        /// A reference tracker to a tile's previous neighbor that updated this tile's distance from a source tile; this value can be 
        /// recursively used to trace the path from a tile to the starting source tile.
        /// </summary>
        public INode PathFrom { get; set; }

        /// <summary>
        /// A reference to a tile's adjacent neighbor in the linked list data structure of the PathfindingNodePriorityQueue object; if this 
        /// property is null, then the cell has no neighbor in the queue.
        /// </summary>
        public INode NextWithSamePriority { get; set; }

        /// <summary>
        /// Tracker of which phase of the search a tile is in; either not yet in the frontier [0], currently part of the frontier [1], or 
        /// behind the frontier [2].
        /// </summary>
        public int SearchPhase { get; set; }
    }

    public static class INodeExtensions
    {
        /// <summary>
        /// (F Cost) Reference to a tile's distance priority for when it should be evaluated in the search relative to other tiles; this 
        /// value is determined by a tile's current distance from the source tile and the search heuristic.
        /// </summary>
        public static int GetSearchPriority(this INode iNode)
        {
            return iNode.Distance + iNode.SearchHeuristic;
        }
    }
}