/* 
 * Copyright (c) 2026 Kokowolo. All Rights Reserved.
 * Author(s): Kokowolo, Will Lacey
 * Date Created: January 25, 2026
 * 
 * Additional Comments:
 *      File Line Length: ~140
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kokowolo.Pathfinding
{
    public interface INodeComposition : INode
    {
        /*██████████████████████████████████████████████████████████*/
        #region Properties

        Node Node { get; }

        /// <summary>
        /// can this node be explored (this is essentially visited if no fog of war)
        /// </summary>
        bool INode.IsExplorable 
        {
            get => Node.IsExplorable;
            set => Node.IsExplorable = value;
        }

        /// <summary>
        /// can this node be visited currently
        /// </summary>
        bool INode.IsVisitable 
        {
            get => Node.IsVisitable;
            set => Node.IsVisitable = value;
        }

        /// <summary>
        /// (G cost) property used for tracking a tile's distance from a source tile
        /// </summary>
        int INode.Distance 
        {
            get => Node.Distance;
            set => Node.Distance = value;
        }

        /// <summary>
        /// (H Cost) reference to a tile's optimal/potential distance from a source tile; this value can be used to gauge the possible 
        /// distance this tile is from the source tile and will return the lowest potential distance cost
        /// </summary>
        int INode.SearchHeuristic 
        {
            get => Node.SearchHeuristic;
            set => Node.SearchHeuristic = value;
        }

        /// <summary>
        /// A reference tracker to a tile's previous neighbor that updated this tile's distance from a source tile; this value can be 
        /// recursively used to trace the path from a tile to the starting source tile
        /// </summary>
        INode INode.PathFrom 
        {
            get => Node.PathFrom;
            set => Node.PathFrom = value;
        }

        /// <summary>
        /// A reference to a tile's adjacent neighbor in the linked list data structure of the PathfindingNodePriorityQueue object; if this 
        /// property is null, then the cell has no neighbor in the queue
        /// </summary>
        INode INode.NextWithSamePriority 
        {
            get => Node.NextWithSamePriority;
            set => Node.NextWithSamePriority = value;
        }

        /// <summary>
        /// tracker of which phase of the search a tile is in; either not yet in the frontier [0], currently part of the frontier [1], or 
        /// behind the frontier [2]
        /// </summary>
        int INode.SearchPhase 
        {
            get => Node.SearchPhase;
            set => Node.SearchPhase = value;
        }

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Functions

        #endregion
        /*██████████████████████████████████████████████████████████*/
    }
}