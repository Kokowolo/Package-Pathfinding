/*
 * File Name: PathfindingNodePath.cs
 * Description: This script is for ...
 * 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: February 13, 2023
 * 
 * Additional Comments:
 *		File Line Length: 120
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using Kokowolo.Utilities;

namespace Kokowolo.Pathfinding
{
    [Serializable]
    public class NodePath : IEnumerable<Node>
    {
        /************************************************************/
        #region Events

        #endregion
        /************************************************************/
        #region Fields

        [SerializeField] private List<Node> list = ListPool.Get<Node>();

        #endregion
        /************************************************************/
        #region Properties

        public int Length => list.Count;
        public bool IsValid => (Length > 1);

        public Node Start => Length > 0 ? list[0] : null;
        public Node Penultimate => IsValid ? list[Length - 2] : null;
        public Node End => Length > 0 ? list[Length - 1] : null;

        public int Distance { get; private set; }

        public Node this[int i]
        {
            get => list[i];
            set => list[i] = value;
        }

        #endregion
        /************************************************************/
        #region Functions

        ~NodePath()
        {
            ListPool.Release(list);
        }

        public void Clear()
        {
            Distance = 0;
            list.Clear();
        }

        public void Copy(List<Node> list)
        {
            Clear();
            foreach (Node node in list) Add(node);
            Distance = End == null ? int.MaxValue : End.Distance;
        }

        public void Copy(NodePath path)
        {
            Copy(path.list);
        }

        public void Add(Node node)
        {
            Distance = node.Distance;
            list.Add(node);
        }

        // public void Remove(PathfindingNode node)
        // {
        //     list.Remove(node);
        // }
        // FIXME: [LUTRO-292] Add PathfindingNodePath Distance Tracking - removed function

        public void RemoveAt(int index)
        {
            list.RemoveAt(index);
            Distance = End.Distance; 
            // FIXME: [LUTRO-292] Add PathfindingNodePath Distance Tracking - this isn't correct because a node's distance could change when RemoveAt is called
        }

        public bool Contains(Node node)
        {
            return list.Contains(node);
        }

        // public void Shift(GridDirection direction)
        // {
        //     for (int i = 0; i < coordinatesList.Count; i++)
        //     {
        //         coordinatesList[i] = coordinatesList[i].GetCoordinatesInDirection(direction);
        //     }
        // }

        public IEnumerator<Node> GetEnumerator()
        {
            foreach (Node node in list)
            {
                yield return node;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            string str = "Path: ";
            if (IsValid)
            {
                for (int i = 0; i < list.Count - 1; i++)
                {
                    str += list[i].ToString() + " -> ";
                }
                str += End.ToString();
            }
            else
            {
                str += "is not valid";
            }
            
            return str;
        }

        #endregion
        /************************************************************/
    }
}