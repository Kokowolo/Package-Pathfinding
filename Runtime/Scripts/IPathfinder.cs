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
    public interface IPathfinder
    {
        /*██████████████████████████████████████████████████████████*/
        #region Functions

        /// <summary>
        /// Gets the approximate distance/move cost between two nodes.
        /// </summary>
        public int GetHeuristicCost(INode from, INode to);

        /// <summary>
        /// Gets the distance/move cost between two nodes.
        /// </summary>
        public int GetMoveCost(INode from, INode to);

        /// <summary>
        /// Gets a new list of neighbors from iNode.
        /// </summary>
        public IEnumerable<INode> GetNeighborsFrom(INode iNode);
        // {
        //     // NOTE: this may change based on pathfinding configuration, i.e. a node's unit using its GetNeighbors() vs what's below
        //     return iNode.GetNeighbors();
        // }

        // /// <summary>
        // /// Efficiently checks to see if the current path is too long (i.e. path.Distance > maxDistance).
        // /// </summary>
        // public bool IsPathOutsideMovementRange(NodePath path);

        /// <summary>
        /// Can the IPathfinder traverse between these two nodes.
        /// </summary>
        public bool IsValidMove(INode from, INode to);
        // {
        //     return start.HasNeighbor(end);
        // }

        #endregion
        /*██████████████████████████████████████████████████████████*/
    }
}