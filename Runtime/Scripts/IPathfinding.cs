/*
 * File Name: IPathfinding.cs
 * Description: This script is for ...
 * 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: February 14, 2023
 * 
 * Additional Comments:
 *		File Line Length: 120
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

        #endregion
        /************************************************************/
        #region Functions

        public List<Node> GetNeighborsFromNode(Node node);

        public bool IsValidMoveBetweenNodes(Node start, Node end);

        public int GetMoveCostBetweenNodes(Node start, Node end);

        public bool IsPathTrimmable(NodePath path);

        // TODO: add burst or jobs to project
        // internal void OnSearchComplete(PathfindingNodePath path);
        
        #endregion
        /************************************************************/
    }
}