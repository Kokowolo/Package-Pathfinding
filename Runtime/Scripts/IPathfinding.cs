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

        // TODO: add burst or jobs to package
        // internal void OnSearchComplete(PathfindingNodePath path);
        
        #endregion
        /*██████████████████████████████████████████████████████████*/
    }
}