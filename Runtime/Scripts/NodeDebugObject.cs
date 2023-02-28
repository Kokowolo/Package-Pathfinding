/*
 * File Name: GridCellDebugObject.cs
 * Description: This script is for ...
 * 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: August 22, 2022
 * 
 * Additional Comments:
 *		File Line Length: 120
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
        /************************************************************/
        #region Fields

        [SerializeField] private TextMeshPro titleText;
        [SerializeField] private TextMeshPro subtitleText;
        [SerializeField] private TextMeshPro gCostText;
        [SerializeField] private TextMeshPro hCostText;
        [SerializeField] private TextMeshPro fCostText;

        #endregion
        /************************************************************/
        #region Properties

        public Node Node { get; private set; }

        #endregion
        /************************************************************/
        #region Functions

        public void Initialize(string title, string subtitle, Node node, Vector3 position, float scale) 
        {
            name = $"Debug Node {title}";
            titleText.text = title;
            subtitleText.text = subtitle;
            Node = node;
            transform.position = position;
            transform.localScale *= scale;

            RefreshGridCellData();
        }

        public void RefreshGridCellData()
        {
            gameObject.SetActive(Node.IsExplorable);
        }

        public void RefreshPathfindingNodeData(int searchFrontierPhase)
        {
            SetTextColor(GetPathfindingColor(searchFrontierPhase));        
        
            if (Node.SearchPhase < searchFrontierPhase)
            {
                gCostText.text = "";
                hCostText.text = "";
                fCostText.text = "";
            }
            else
            {
                gCostText.text = "G:" + Node.Distance.ToString();
                hCostText.text = "H:" + Node.SearchHeuristic.ToString();
                fCostText.text = "F:" + Node.SearchPriority.ToString();
            }
        }

        private void SetTextColor(Color color)
        {
            titleText.color = color;
            subtitleText.color = color;
            gCostText.color = color;
            hCostText.color = color;
            fCostText.color = color;
        }

        private Color GetPathfindingColor(int searchFrontierPhase)
        {
            if (!Node.IsWalkable) 
            {
                return Color.red;
            }
            else if (Node.SearchPhase == searchFrontierPhase)
            {
                return Color.green;
            }
            else if (Node.SearchPhase > searchFrontierPhase)
            {
                return Color.yellow;
            }
            else
            {
                return Color.white;
            }
        }

        #endregion
        /************************************************************/
    }
}