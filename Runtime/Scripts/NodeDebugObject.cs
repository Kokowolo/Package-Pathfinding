/*
 * Copyright (c) 2026 Kokowolo. All Rights Reserved. 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: August 22, 2022
 * 
 * Additional Comments:
 *		File Line Length: ~140
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.EventSystems;

namespace Kokowolo.Pathfinding
{
    public class NodeDebugObject : MonoBehaviour
    {
        /*██████████████████████████████████████████████████████████*/
        #region Fields

        [SerializeField] TextMeshPro titleText;
        [SerializeField] TextMeshPro subtitleText;
        [SerializeField] TextMeshPro gCostText;
        [SerializeField] TextMeshPro hCostText;
        [SerializeField] TextMeshPro fCostText;

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Properties

        public INode INode { get; private set; }

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Functions

        public void Initialize(INode iNode, string title, string subtitle,  float scale) 
        {
            INode = iNode;

            name = $"Debug Node {title}";
            titleText.text = title;
            subtitleText.text = subtitle;
            transform.localScale *= scale;
        }

        public void Refresh(Vector3 position, Quaternion rotation, int searchFrontierPhase)
        {
            transform.position = position;
            transform.rotation = rotation;
            gameObject.SetActive(INode.IsExplorable);
            
            SetTextColor(GetPathfindingColor(searchFrontierPhase));        
        
            if (INode.SearchPhase < searchFrontierPhase)
            {
                gCostText.text = "";
                hCostText.text = "";
                fCostText.text = "";
            }
            else
            {
                gCostText.text = "G:" + INode.Distance.ToString();
                hCostText.text = "H:" + INode.SearchHeuristic.ToString();
                fCostText.text = "F:" + INode.GetSearchPriority().ToString();
            }
        }

        void SetTextColor(Color color)
        {
            titleText.color = color;
            subtitleText.color = color;
            gCostText.color = color;
            hCostText.color = color;
            fCostText.color = color;
        }

        Color GetPathfindingColor(int searchFrontierPhase)
        {
            if (!INode.IsVisitable) 
            {
                return Color.red;
            }
            else if (INode.SearchPhase == searchFrontierPhase)
            {
                return Color.green;
            }
            else if (INode.SearchPhase > searchFrontierPhase)
            {
                return Color.yellow;
            }
            else
            {
                return Color.white;
            }
        }

        #endregion
        /*██████████████████████████████████████████████████████████*/
    }
}