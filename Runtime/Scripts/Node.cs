/*
 * Copyright (c) 2026 Kokowolo. All Rights Reserved. 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: August 25, 2022
 * 
 * Additional Comments:
 *		File Line Length: ~140
 *
 *      // HACK: this class should be a struct and it should be on the implementation of INode to handle List<INode> neighbors adjacency
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using Kokowolo.Utilities;

namespace Kokowolo.Pathfinding
{
    public class Node : INode, IDisposable
    {
        /*██████████████████████████████████████████████████████████*/
        #region Fields

        public List<INode> neighbors;

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Properties

        public bool IsExplorable { get; set; } = true;
        public bool IsVisitable { get; set;  } = true;
        public int Distance { get; set; }
        public int SearchHeuristic { get; set; }
        public INode PathFrom { get; set; }
        public INode NextWithSamePriority { get; set; }
        public int SearchPhase { get; set; }

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Functions

        public Node()
        {
            neighbors = ListPool.Get<INode>();
        }

        bool disposed;
        ~Node() => Dispose();
        public void Dispose()
        {
            if (disposed) return;
            disposed = true;
            GC.SuppressFinalize(this);
            ListPool.Add(neighbors);
        }

        #endregion
        /*██████████████████████████████████████████████████████████*/
    }
}