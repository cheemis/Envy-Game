using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Node : MonoBehaviour
{
    [SerializeField]
    private Node[] neighbors = new Node[4];
    private Vector3 position;

    public Color lineColor  = Color.white;


    private void Awake()
    {
        position = transform.position;
    }

    public Node GetNeighbor(Vector2 direction)
    {
        if (direction == Vector2.up) return neighbors[0];

        if (direction == Vector2.down) return neighbors[1];

        if (direction == Vector2.right) return neighbors[2];

        if (direction == Vector2.left) return neighbors[3];

        return null;
    }

    public Node GetNeighbor(int index)
    {
        if(index < neighbors.Length) return neighbors[index];
        return null;
    }

    public void SetNeighbor(int index, Node neighbor)
    {
        if(index >= neighbors.Length) return;
        neighbors[index] = neighbor;
    }


    public void SetLineColor(Color newColor)
    {
        lineColor = newColor;
    }

    public Vector3 GetPosition()
    {
        if(position == Vector3.zero) position = transform.position;
        return position;
    }

    
    void OnDrawGizmos()
    {
        Vector3 currPos = transform.position;

        for (int i = 0; i < neighbors.Length; i++)
        {
            
            if (neighbors[i] != null)
            {
                Vector3 neighborPos = neighbors[i].transform.position;

                //draw a line from the current object to half way to next object
                //Gizmos.color = lineColor;
                Gizmos.DrawLine(currPos, currPos + (neighborPos - currPos) / 2);
            }
        }
    }
}
