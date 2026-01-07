/*
 * Copyright (c) 2026 Kokowolo. All Rights Reserved. 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: February 14, 2023
 * 
 * Additional Comments:
 *		File Line Length: ~140
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kokowolo.Pathfinding
{
    public interface IPathfinding
    {
        /*██████████████████████████████████████████████████████████*/
        #region Properties

        public bool CanCreatePathsWithRepeatNodes { get; }

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Functions

        // NOTE: this may change based on pathfinding configuration, i.e. a node's unit using its GetNeighbors() vs what's below
        public List<INode> GetNeighborsFromNode(INode iNode)
        {
            return iNode.Node.GetNeighbors();
        }

        public bool IsValidMoveBetweenNodes(INode start, INode end)
        {
            return start.Node.HasNeighbor(end);
        }

        // NOTE: this should get the distance/approximate between these two nodes
        public int GetHeuristicCostBetweenNodes(INode start, INode end);

        // NOTE: this should get the actual distance between these two nodes
        public int GetMoveCostBetweenNodes(INode start, INode end);

        // NOTE: this method is a hacky way of checking if the current path is too long; i.e. path.Distance > maxDistance;
        public bool IsPathOutsideMovementRange(INodePath path)
        {
            return false;
        }

        // TODO: add burst or jobs to package
        // internal void OnSearchComplete(PathfindingNodePath path);
        
        #endregion
        /*██████████████████████████████████████████████████████████*/
    }
}