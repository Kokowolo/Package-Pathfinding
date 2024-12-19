/*
 * File Name: INode.cs
 * Description: This script is for ...
 * 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: December 19, 2024
 * 
 * Additional Comments:
 *      File Line Length: 140
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kokowolo.Pathfinding
{
    public interface INode
    {
        /************************************************************/
        #region Properties

        public Node Node { get; }

        #endregion
        /************************************************************/
    }
    
    public interface INode<T> : INode
    {
        /************************************************************/
        #region Properties

        public new Node<T> Node { get; }
        Node INode.Node => Node;

        #endregion
        /************************************************************/
    }
}