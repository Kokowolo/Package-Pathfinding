/*
 * Author(s): Kokowolo, Will Lacey
 * Date Created: February 14, 2023
 * 
 * Additional Comments:
 *      File Line Length: 140
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
        public List<Node> GetNeighborsFromNode(Node node)
        {
            return node.GetNeighbors();
        }

        public bool IsValidMoveBetweenNodes(Node start, Node end)
        {
            return start.HasNeighbor(end);
        }

        // NOTE: this should get the distance/approximate between these two nodes
        public int GetHeuristicCostBetweenNodes(Node start, Node end);

        // NOTE: this should get the actual distance between these two nodes
        public int GetMoveCostBetweenNodes(Node start, Node end);

        // NOTE: this method is a hacky way of checking if the current path is too long; i.e. path.Distance > maxDistance;
        public bool IsPathOutsideMovementRange(NodePath path)
        {
            return false;
        }

        // TODO: add burst or jobs to package
        // internal void OnSearchComplete(PathfindingNodePath path);
        
        #endregion
        /*██████████████████████████████████████████████████████████*/
    }
}