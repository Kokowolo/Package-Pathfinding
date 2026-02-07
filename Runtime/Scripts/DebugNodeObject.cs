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
using Kokowolo.Utilities;

namespace Kokowolo.Pathfinding
{
    public class DebugNodeObject : MonoBehaviour, IPoolableMonoBehaviour
    {
        /*██████████████████████████████████████████████████████████*/
        #region Fields

        [SerializeField] TextMeshProUGUI titleText;
        [SerializeField] TextMeshProUGUI subtitleText;
        [SerializeField] TextMeshProUGUI gCostText;
        [SerializeField] TextMeshProUGUI hCostText;
        [SerializeField] TextMeshProUGUI fCostText;

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Properties

        public INode INode { get; private set; }

        public static Color ColorIsNotExplorable = new Color(1, 0, 0, 0.25f); // red
        public static Color ColorSearchFrontier = new Color(0, 1, 0, 0.7f); // green
        public static Color ColorBehindSearchFrontier = new Color(1f, 0.92f, 0.016f, 0.7f); // yellow
        public static Color ColorUnsearched = new Color(1, 1, 1, 0.25f); // white

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Functions

        public void Initialize(INode iNode, string title, string subtitle,  float scale) 
        {
            INode = iNode;

            name = $"DebugNode {title} ({subtitle})";
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

        public void SetTitleTextColor(Color color)
        {
            titleText.color = color;
            subtitleText.color = color;
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
            if (!INode.IsExplorable) 
            {
                return ColorIsNotExplorable;
            }
            else if (INode.SearchPhase == searchFrontierPhase)
            {
                return ColorSearchFrontier;
            }
            else if (INode.SearchPhase > searchFrontierPhase)
            {
                return ColorBehindSearchFrontier;
            }
            else
            {
                return ColorUnsearched;
            }
        }

        void IPoolable.OnAddedToPool()
        {
            gameObject.SetActive(false);
        }

        #endregion
        /*██████████████████████████████████████████████████████████*/
    }
}