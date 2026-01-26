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
        /// Gets a new list of neighbors from iNode.
        /// </summary>
        public IEnumerable<INode> GetNeighborsFromNode(INode iNode);
        // {
        //     // NOTE: this may change based on pathfinding configuration, i.e. a node's unit using its GetNeighbors() vs what's below
        //     return iNode.GetNeighbors();
        // }

        /// <summary>
        /// Can the IPathfinder traverse between these two nodes.
        /// </summary>
        public bool IsValidMoveBetweenNodes(INode start, INode end);
        // {
        //     return start.HasNeighbor(end);
        // }

        /// <summary>
        /// Gets the approximate distance/move cost between two nodes.
        /// </summary>
        public int GetHeuristicCostBetweenNodes(INode start, INode end);

        /// <summary>
        /// Gets the distance/move cost between two nodes.
        /// </summary>
        public int GetMoveCostBetweenNodes(INode start, INode end);

        /// <summary>
        /// Efficiently checks to see if the current path is too long (i.e. path.Distance > maxDistance).
        /// </summary>
        public bool IsPathOutsideMovementRange(NodePath path)
        {
            return false;
        }

        #endregion
        /*██████████████████████████████████████████████████████████*/
    }
}