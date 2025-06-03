/*
 * Author(s): Kokowolo, Will Lacey
 * Date Created: April 17, 2023
 * 
 * Additional Comments:
 *		File Line Length: ~140
 */

using System;
using System.Collections.Generic;
using Kokowolo.Utilities;

namespace Kokowolo.Pathfinding
{
    [Serializable]
    public class Node<T> : Node where T : INode
    {
        /*██████████████████████████████████████████████████████████*/
        #region Properties

        public T Owner { get; private set; }

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Functions

        public Node(T owner)
        { 
            Owner = owner;
            neighbors = ListPool.Get<Node>();
        }

        // HACK: this is so PathfindingVisual can create duplicate nodes with independent Distance values; can this be cleaned up?
        // public Node(Node node)
        // {
        //     Instance = node.Instance;
        //     neighbors = ListPool.Get<Node>(node.neighbors);
        //     IsExplorable = node.IsExplorable;
        //     Distance = node.Distance;
        //     SearchHeuristic = node.SearchHeuristic;
        //     PathFrom = node.PathFrom;
        //     NextWithSamePriority = node.NextWithSamePriority;
        //     SearchPhase = node.SearchPhase;
        // }

        ~Node()
        {
            ListPool.Add(neighbors);
        }

        public override string ToString()
        {
            return $"({Distance}) {Owner}";
        }

        #endregion
        /*██████████████████████████████████████████████████████████*/
    }
}