using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.GraphicsBuffer;

public class GraphManager : MonoBehaviour
{
    [SerializeField]
    private Dictionary<Vector2, GameObject> nodes = new Dictionary<Vector2, GameObject>();

    public Tilemap tilemap;
    public Sprite outOfBoundsSprite;
    private float tileSize;

    [SerializeField]
    private GameObject node;
    [SerializeField]
    private GameObject firstNode;


    private Vector3[] directions = { Vector3.up, Vector3.down, Vector3.right, Vector3.left };

    private void Start()
    {
        //get the size of the tiles in the tilemap
        tileSize = tilemap.cellSize.x;


        node.transform.position = GetTilePosition(node.transform.position);

        CreateGraph();

    }

    private void CreateGraph()
    {
        
        Queue<GameObject> currentNodes = new Queue<GameObject>();

        currentNodes.Enqueue(firstNode);
        nodes.Add(GetTilePosition(firstNode.transform.position), firstNode);

        //put all the nodes on the map
        while (currentNodes.Count > 0)
        {
            GameObject current = currentNodes.Dequeue();
            Node currentNode = current.GetComponent<Node>();

            //see if neighbors are valid
            for(int i = 0; i < 4; i++)
            {
                Vector3 potentialNeighbor = GetTilePosition(current.transform.position + directions[i]);

                if (potentialNeighbor != Vector3.zero && !nodes.ContainsKey(potentialNeighbor))
                {
                    //create a new node
                    GameObject newNeighbor = Instantiate(node, potentialNeighbor, node.transform.rotation);

                    int oppositeDirection = i == 0 ? 1 :
                                            i == 1 ? 0 :
                                            i == 2 ? 3 : 2;

                    //set the node as a neighbor
                    currentNode.SetNeighbor(i, newNeighbor);
                    newNeighbor.GetComponent<Node>().SetNeighbor(oppositeDirection, current);

                    currentNodes.Enqueue(newNeighbor);
                    nodes.Add(potentialNeighbor, newNeighbor);
                }
            }
        }

        //profit

    }


    private Vector3 GetTilePosition(Vector3 target)
    {

        //get the sprite of the next tile to visit
        Vector3Int cellLocation = tilemap.WorldToCell(target);
        Sprite tileSprite = tilemap.GetSprite(cellLocation);

        //if the tile exists and its not out of bounds
        if (tileSprite != null && tileSprite != outOfBoundsSprite)
        {
            return tilemap.GetCellCenterWorld(cellLocation);
        }

        //else, return empty
        return Vector3.zero;
    }

}
