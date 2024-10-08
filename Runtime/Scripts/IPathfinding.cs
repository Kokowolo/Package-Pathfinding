/*
 * File Name: IPathfinding.cs
 * Description: This script is for ...
 * 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: February 14, 2023
 * 
 * Additional Comments:
 *      File Line Length: 120
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kokowolo.Pathfinding
{
    public interface IPathfinding
    {
        /************************************************************/
        #region Properties

        public bool CanCreatePathsWithRepeatNodes { get; }

        #endregion
        /************************************************************/
        #region Functions

        public List<Node> GetNeighborsFromNode(Node node);

        public bool IsValidMoveBetweenNodes(Node start, Node end);

        public int GetHeuristicCostBetweenNodes(Node start, Node end);

        public int GetMoveCostBetweenNodes(Node start, Node end);

        public bool IsPathOutsideMovementRange(NodePath path);

        // TODO: add burst or jobs to project
        // internal void OnSearchComplete(PathfindingNodePath path);
        
        #endregion
        /************************************************************/
    }
}