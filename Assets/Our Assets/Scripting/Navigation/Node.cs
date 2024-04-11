using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField]
    private GameObject[] neighbors = new GameObject[4];


    public GameObject GetNeighbor(Vector2 direction)
    {
        if (direction == Vector2.up) return neighbors[0];

        if (direction == Vector2.down) return neighbors[1];

        if (direction == Vector2.right) return neighbors[2];

        if (direction == Vector2.left) return neighbors[3];

        return null;
    }

    public void SetNeighbor(int index, GameObject neighbor)
    {
        if(index >= neighbors.Length) return;
        neighbors[index] = neighbor;
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
                Gizmos.DrawLine(currPos, currPos + (neighborPos - currPos) / 2);
            }
        }
    }
}
