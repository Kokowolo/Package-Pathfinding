/*
 * Copyright (c) 2026 Kokowolo. All Rights Reserved. 
 * Author(s): Catlike Coding, Kokowolo, Will Lacey
 * Date Created: September 29, 2020
 * 
 * Additional Comments:
 *      The original version of this file can be found here: https://catlikecoding.com/unity/tutorials/hex-map/ within 
 *      Catlike Coding's tutorial series: Hex Map; this file has been updated it to better fit this project
 *
 *		File Line Length: ~140
 */

using System.Collections.Generic;

namespace Kokowolo.Pathfinding
{
    /// <summary>
    /// Class for containing a priority queue data-structure that is specifically tailored to the `Pathfinding.Node` object; useful for 
    /// calculating distances and paths among `Pathfinding.Node`s
    /// </summary>
    public class NodePriorityQueue
    {
        /*██████████████████████████████████████████████████████████*/
        #region Fields

        /// <summary>
        /// core data structure variable
        /// </summary>
        List<INode> priorityQueue = new List<INode>();

        /// <summary>
        /// value to keep track of the minimum node priority
        /// </summary>
        int minimum = int.MaxValue;

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Properties

        /// <summary>
        /// Gets the current number of node elements within the Priority Queue
        /// </summary>
        public int Count { get; private set; }

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Functions

        /// <summary>
        /// Adds an INode to the priority queue
        /// </summary>
        public void Enqueue(INode iNode)
        {
            Count += 1;
            int priority = iNode.GetSearchPriority();

            // add null elements into the list until the count matches the node's priority
            while (priority >= priorityQueue.Count) priorityQueue.Add(null);

            // this creates a linked list of nodes; the structure of filling the list with empty node and adding a linked 
            // linked list to existing indices looks like this: http://bit.ly/HexPriorityQueue
            iNode.NextWithSamePriority = priorityQueue[priority];

            // potentially update the minimum
            if (priority < minimum) minimum = priority;

            // sets the node to the front of the priority queue
            priorityQueue[priority] = iNode;
        }

        /// <summary>
        /// Removes the next node in the priority queue
        /// </summary>
        /// <returns>the removed node</returns>
        public INode Dequeue()
        {
            Count -= 1;

            // find the first node that isn't null in the list and return
            while (minimum < priorityQueue.Count)
            {
                INode iNode = priorityQueue[minimum];
                if (iNode != null)
                {
                    // decrement the list at this index by setting the next node to the front of list
                    priorityQueue[minimum] = iNode.NextWithSamePriority;
                    return iNode;
                }
            minimum++; // increment the new minimum and find the new lowest priority node
            }

            return null; // list is empty
        }

        /// <summary>
        /// Updates an existing node in the queue to its new value given its old value
        /// </summary>
        /// <param name="iNode">the node to update</param>
        /// <param name="oldPriority">the node's old priority value</param>
        public void Change(INode iNode, int oldPriority)
        {
            INode current = priorityQueue[oldPriority];
            INode next = current.NextWithSamePriority; // this could be null 

            // fix list after removing node logic
            if (current == iNode)
            {
                priorityQueue[oldPriority] = next; // decrement the link list at this index
            }
            else
            {
                // keep searching linked list until 'next' is the node
                while (next != iNode)
                {
                    current = next;
                    next = current.NextWithSamePriority;
                }

                // 'next' is the node, so we can remove/pop 'next' and set 'current.Next...' to
                // 'next.Next...'
                current.NextWithSamePriority = iNode.NextWithSamePriority;
            }

            // we've updated the list after removing the node, now let's readd the node...
            Enqueue(iNode);
            Count -= 1; // ...while keeping the count the same
        }

        /// <summary>
        /// Completely clears the priority queue and resets its values
        /// </summary>
        public void Clear()
        {
            priorityQueue.Clear();
            Count = 0;
            minimum = int.MaxValue;
        }

        #endregion
        /*██████████████████████████████████████████████████████████*/
    }
}