/*
 * Copyright (c) 2026 Kokowolo. All Rights Reserved. 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: February 13, 2023
 * 
 * Additional Comments:
 *		File Line Length: ~140
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using Kokowolo.Utilities;

namespace Kokowolo.Pathfinding
{
    [Serializable]
    public class NodePath : IEnumerable<INode>, IDisposable
    {
        /*██████████████████████████████████████████████████████████*/
        #region Fields

        [SerializeField] List<INode> list;
        [SerializeField] List<int> distances;

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Properties

        public int Length => list.Count;
        public bool IsValid => Length > 1;

        public INode Start => Length > 0 ? list[0] : null;
        public INode Penultimate => IsValid ? list[Length - 2] : null;
        public INode End => Length > 0 ? list[Length - 1] : null;

        public int Distance { get; private set; }

        public INode this[int i]
        {
            get => list[i];
            set => list[i] = value;
        }

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Functions

        public NodePath()
        {
            list = ListPool.Get<INode>();
            distances = ListPool.Get<int>();
        }

        bool disposed;
        ~NodePath() => Dispose();
        public void Dispose()
        {
            if (disposed) return;
            disposed = true;
            GC.SuppressFinalize(this);
            ListPool.Add(list);
            ListPool.Add(distances);
        }

        public void Clear()
        {
            list.Clear();
            distances.Clear();
            Distance = 0;
        }

        internal void Copy(List<INode> list)
        {
            Clear();
            if (list.Count > 0) Add(list[0], 0);

            for (int i = 1; i < list.Count; i++)
            {
                int distance = list[i].Distance - list[i - 1].Distance;
                Add(list[i], distance);
            }
        }

        public void Copy(NodePath path)
        {
            Clear();
            for (int i = 0; i < path.Length; i++)
            {
                Add(path[i], path.distances[i]);
            }
        }

        internal void Add(INode node, int distance)
        {
            Distance += distance;
            distances.Add(distance);
            list.Add(node);
        }

        public void RemoveAt(int index)
        {
            Distance -= distances[index]; 
            distances.RemoveAt(index);
            list.RemoveAt(index);
        }

        public bool Contains(INode node)
        {
            return list.Contains(node);
        }

        public IEnumerator<INode> GetEnumerator()
        {
            foreach (INode node in list)
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
        /*██████████████████████████████████████████████████████████*/
    }
}