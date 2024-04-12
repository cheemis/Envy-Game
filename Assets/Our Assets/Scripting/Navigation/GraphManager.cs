using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.GraphicsBuffer;

public class GraphManager : MonoBehaviour
{

    // ================================================================ //
    // =========================== Variables ========================== //
    // ================================================================ //

    //node variables
    [SerializeField]
    private Dictionary<Vector2, GameObject> nodes = new Dictionary<Vector2, GameObject>();
    [SerializeField]
    private GameObject nodePrefab;
    [SerializeField]
    private GameObject firstNode;

    //tilemap variables
    public Tilemap tilemap;
    public Sprite outOfBoundsSprite;
    private float tileSize;

    private Color[] colors = { Color.red, Color.white, Color.green, Color.blue, Color.cyan };
    private int colorIndex = 0;


    private Vector3[] directions = { Vector3.up, Vector3.down, Vector3.right, Vector3.left };

    private void Start()
    {
        //get the size of the tiles in the tilemap
        tileSize = tilemap.cellSize.x;


        firstNode.transform.position = GetTilePosition(firstNode.transform.position);

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

                //if a valid tile
                if (potentialNeighbor != Vector3.zero)
                {
                    //get the direction that this node is from the found neighbor
                    int oppositeDirection = i == 0 ? 1 :
                                            i == 1 ? 0 :
                                            i == 2 ? 3 : 2;

                    GameObject foundNeighbor;

                    //if tile doesn't already exists
                    if (!nodes.TryGetValue(potentialNeighbor, out foundNeighbor))
                    {
                        //create a new node
                        foundNeighbor = Instantiate(nodePrefab, potentialNeighbor, nodePrefab.transform.rotation);

                        //add this node to the queue and the dictionary of nodes
                        currentNodes.Enqueue(foundNeighbor);
                        nodes.Add(potentialNeighbor, foundNeighbor);
                    }

                    //set the node as a neighbor
                    currentNode.SetNeighbor(i, foundNeighbor);
                    foundNeighbor.GetComponent<Node>().SetNeighbor(oppositeDirection, current);


                    //testing gizmo
                    Color randColor = colors[colorIndex];
                    colorIndex = (colorIndex + 1) % colors.Length;
                    foundNeighbor.GetComponent<Node>().SetLineColor(randColor);
                }
            }
        }
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

    public Vector3 GetNodePosition(Vector3 worldPosition)
    {
        Vector3 tilePosition = GetTilePosition(worldPosition);

        return tilePosition;
    }

}
