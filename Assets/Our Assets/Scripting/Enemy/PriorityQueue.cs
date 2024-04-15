using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PriorityQueue
{
    // ================================================================ //
    // ======================= Internal Classes ======================= //
    // ================================================================ //
    private class QueueElement
    {
        private Node node;
        private float priority;

        public QueueElement(Node node, float priority)
        {
            this.node = node;
            this.priority = priority;
        }

        public Node GetNode() { return node; }
        public float GetPriority() { return priority; }
    }

    // ================================================================ //
    // =========================== Variables ========================== //
    // ================================================================ //

    private List<Node> queue;
    private Dictionary<Vector2, float> priorityMap = new Dictionary<Vector2, float>();


    // ================================================================ //
    // ==================== Instantiation Functions =================== //
    // ================================================================ //

    public PriorityQueue()
    {
        queue = new List<Node>();
        priorityMap = new Dictionary<Vector2, float>();
    }

    // ================================================================ //
    // ======================= Public Functions ======================= //
    // ================================================================ //

    public void Insert(Node newNode, float newPriority)
    {
        QueueElement newElement = new QueueElement(newNode, newPriority);

        //insert the new elment into the queue - replace later with better search
        for(int i = 0; i < queue.Count; i++)
        {
            float priority = 0;
            priorityMap.TryGetValue(queue[i].GetPosition(), out priority);

            if (priority > newPriority)
            {
                //found a location in the queue
                queue.Insert(i, newNode);
                priorityMap.Add(newNode.GetPosition(), newPriority);
                return;
            }
        }
        //biggest element, insert at the end
        queue.Add(newNode);
        priorityMap.Add(newNode.GetPosition(), newPriority);
    }


    public Node Dequeue()
    {
        //edge case
        if(queue.Count == 0) return null;

        //get and remove first element
        Node firstElement = queue[0];
        queue.RemoveAt(0);
        priorityMap.Remove(firstElement.GetPosition());

        //return the first element
        return firstElement;
    }


    // ================================================================ //
    // ======================= Getters / Setters ====================== //
    // ================================================================ //

    public int Count() { return queue.Count; }
    public bool Contains(Node lookingFor) { return queue.Contains(lookingFor) ? true : false; }
}
