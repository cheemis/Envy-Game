using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Collections;
using Unity.VisualScripting;
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
    private Dictionary<Vector2, Node> nodes = new Dictionary<Vector2, Node>();
    [SerializeField]
    private GameObject nodePrefab;
    [SerializeField]
    private GameObject firstNode;

    private Dictionary<Vector2, Pellet> pellets = new Dictionary<Vector2, Pellet>();
    [SerializeField]
    GameObject pelletPrefab;

    //tilemap variables
    public Tilemap tilemap;
    public Sprite outOfBoundsSprite;
    private float tileSize;

    private Color[] colors = { Color.red, Color.white, Color.green, Color.blue, Color.cyan, Color.magenta, Color.yellow, Color.black };
    private int colorIndex = 0;


    private Vector3[] directions = { Vector3.up, Vector3.down, Vector3.right, Vector3.left };

    public int nearestNodePercentage = 50;



    // ================================================================ //
    // =================== Built In Unity Functions =================== //
    // ================================================================ //

    private void Awake()
    {
        //get the size of the tiles in the tilemap
        tileSize = tilemap.cellSize.x;


        firstNode.transform.position = GetTilePosition(firstNode.transform.position);

        CreateGraph();

    }



    // ================================================================ //
    // ======================= Private Functions ====================== //
    // ================================================================ //


    private void CreateGraph()
    {
        
        Queue<Node> currentNodes = new Queue<Node>();

        currentNodes.Enqueue(firstNode.GetComponent<Node>());
        nodes.Add(GetTilePosition(firstNode.transform.position), firstNode.GetComponent<Node>());

        //put all the nodes on the map
        while (currentNodes.Count > 0)
        {
            Node currentNode = currentNodes.Dequeue();

            GameObject newPellet = Instantiate(pelletPrefab, currentNode.GetPosition(), pelletPrefab.transform.rotation);
            pellets.Add(currentNode.GetPosition(), newPellet.GetComponent<Pellet>());

            //see if neighbors are valid
            for (int i = 0; i < 4; i++)
            {
                Vector3 potentialNeighbor = GetTilePosition(currentNode.GetPosition() + directions[i]);

                //if a valid tile
                if (potentialNeighbor != Vector3.zero)
                {
                    //get the direction that this node is from the found neighbor
                    int oppositeDirection = i == 0 ? 1 :
                                            i == 1 ? 0 :
                                            i == 2 ? 3 : 2;


                    Node foundNeighbor;

                    //if tile doesn't already exists
                    if (!nodes.TryGetValue(potentialNeighbor, out foundNeighbor))
                    {
                        //create a new node
                        GameObject neighbor = Instantiate(nodePrefab, potentialNeighbor, nodePrefab.transform.rotation);
                        foundNeighbor = neighbor.GetComponent<Node>();

                        //add this node to the queue and the dictionary of nodes
                        currentNodes.Enqueue(foundNeighbor);
                        nodes.Add(potentialNeighbor, foundNeighbor);
                    }

                    //set the node as a neighbor
                    currentNode.SetNeighbor(i, foundNeighbor);
                    foundNeighbor.GetComponent<Node>().SetNeighbor(oppositeDirection, currentNode);


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



    // ================================================================ //
    // ======================= Public Functions ======================= //
    // ================================================================ //


    public Vector3 GetNodePosition(Vector3 worldPosition)
    {
        Vector3 tilePosition = GetTilePosition(worldPosition);

        return tilePosition;
    }


    public Node GetNode(Vector3 worldPosition)
    {
        Vector3 tilePosition = GetTilePosition(worldPosition);
        Node currentNode;

        if (tilePosition != Vector3.zero && nodes.TryGetValue(worldPosition, out currentNode))
        {
            return currentNode;
        }

        return null;
    }


    public void DeleteThePelletOnNode(Vector2 position, bool isPlayer)
    {
        bool exist = pellets.ContainsKey(position);
        if (exist)
        {
            Pellet p = pellets[position];

            // add the score to player
            if(isPlayer)
            {
                GameManager.Instance.AddPlayerMoney(p.GetScore());
            }
            else
            {
                GameManager.Instance.AddEnemyMoney(p.GetScore());
            }
            
            // remove
            pellets.Remove(position);

            // if the pellets is empty, we can end the game and go to the upgrading page.
            if (pellets.Count < 1) //hack 40 for debugging
            {
                GameManager.Instance.FinishCurrentGame();
                return;
            }

            Destroy(p.gameObject);
        }
    }


    public Node GetRandomNode()
    {

        if (pellets.Count == 0) return null;

        Vector3 randomPellet = pellets.ElementAt(Random.Range(0, pellets.Count)).Value.transform.position;
        Node pelletNode;
        if (nodes.TryGetValue(randomPellet, out pelletNode))
        {
            return pelletNode;
        }
        return null;
    }

    //Use BFS to search for Node has pellet. Start from current position
    public Node GetNearestNode(Node currentNode)
    {
        if (pellets.Count == 0) return null;

        Queue<Node> queue = new Queue<Node>();

        // defines the node has been to 
        HashSet<Vector2> been = new HashSet<Vector2>();

        queue.Enqueue(currentNode);

        while(queue.Count != 0)
        {
            Node temp = queue.Dequeue();

            if (pellets.ContainsKey(temp.GetPosition()))
            {
                return temp;
            }

            been.Add(temp.GetPosition());


            for (int i = 0; i < 4; i++)
            {
                Node child = temp.GetNeighbor(i);

                if (child == null) continue;

                if(been.Contains(child.GetPosition()))
                {
                    continue;
                }

                queue.Enqueue(child);
            }
        }

        Debug.Log("No pellet in the game");


        return null;
    }

    public Node GetRandomWithNearestNode(Node currentNode)
    {
        int randomNumber = Random.Range(0, 100);

        if (randomNumber <= nearestNodePercentage)
        {
            //use nearest node
            return GetNearestNode(currentNode);
        }
        return GetRandomNode();
    }

}

