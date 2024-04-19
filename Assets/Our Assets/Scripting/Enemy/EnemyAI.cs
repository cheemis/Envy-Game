using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    // ================================================================ //
    // =========================== Variables ========================== //
    // ================================================================ //

    //Grid
    [SerializeField]
    private GraphManager graphManager;
    [SerializeField]
    private Node lastNode;
    [SerializeField]
    private Node destinationNode;

    //movement variables
    [SerializeField]
    private float speed = 10;
    [SerializeField]
    private float knockBackTime = 0;

    //components
    private Animator anim;

    //Pathing Variables
    public Queue<Node> path = new Queue<Node>();


    //testing variables
    public int depth;
    private HashSet<Vector2> visited = new HashSet<Vector2>();


    private void OnDrawGizmos()
    {
        Node[] pathArray = path.ToArray();

        for(int i = 1; i < pathArray.Length; i++)
        {
            Gizmos.DrawLine(pathArray[i - 1].GetPosition(), pathArray[i].GetPosition());
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        InitializeEnemy();
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGameStart())
        {
            return;
        }

        if (knockBackTime <= 0)
        {
            MoveCharacter();
        }
        else
        {
            knockBackTime -= Time.deltaTime;
            anim.SetFloat("knockBackTime", knockBackTime);
        }
    }


    // ================================================================ //
    // ======================= Private Functions ====================== //
    // ================================================================ //

    private void InitializeEnemy()
    {
        //set the initial position of the Enemy
        Vector3 tilePosition = graphManager.GetNodePosition(transform.position);
        transform.position = tilePosition;
        lastNode = graphManager.GetNode(tilePosition);
        destinationNode = lastNode;
    }


    /// <summary>
    /// This function moves the player physically. This function should
    /// be called in FixedUpdate()
    /// </summary>
    private void MoveCharacter()
    {
        Vector2 worldSpace = new Vector2(transform.position.x,
                                         transform.position.y);

        Vector3 destination = destinationNode.GetPosition();

        float distance = Vector2.Distance(worldSpace, destination);

        transform.position = Vector3.Lerp(worldSpace, destination, (speed / (distance * 10)) * Time.deltaTime); // MAGIC NUMBER

        if (distance < 0.1f)
        {
            graphManager.DeleteThePelletOnNode(destination, false);
            UpdateDestination();
        }
    }


    private void CreatePath()
    {
        Node newTarget = graphManager.GetRandomWithNearestNode(lastNode);

        if (newTarget == null) return; //usually won't get this case

        //clear visited from previous call
        visited.Clear();

        Stack<Node> stack = AStarSearch(destinationNode, newTarget);

        //couldn't find path
        if (stack == null)
        {
            return;
        }

        while(stack.Count > 0)
        {
            Node popped = stack.Pop();
            path.Enqueue(popped);
        }
    }


    private Stack<Node> DepthFirstSearch(Node currentNode, Node lookingFor)
    {
        //base case: dead end
        if(currentNode == null || visited.Contains(currentNode.GetPosition()))
        {
            return null;
        }

        //base case: found target
        if(currentNode == lookingFor)
        {
            Stack<Node> returningQueue = new Stack<Node>();
            returningQueue.Push(currentNode);

            return returningQueue;
        }

        //visit the current node
        visited.Add(currentNode.GetPosition());

        //run DFS on children
        for (int i = 0; i < 4; i++)
        {
            Stack<Node> childStack = DepthFirstSearch(currentNode.GetNeighbor(i), lookingFor);

            //if found, return stack
            if(childStack != null)
            {
                childStack.Push(currentNode);
                return childStack;
            }
        }
        return null;
    }

    private Stack<Node> AStarSearch(Node startNode, Node lookingFor)
    {
        
        //the map of where the path came from
        Dictionary<Vector2, Node> cameFrom = new Dictionary<Vector2 , Node>();

        //set up gScores map
        Dictionary<Vector2, float> gScore = new Dictionary<Vector2 , float>();
        gScore.Add(startNode.GetPosition(), Distance(startNode, lookingFor)); //initial space has a score of zero

        //set up queue prioritized search
        PriorityQueue openQueue = new PriorityQueue();
        float startScore = float.PositiveInfinity;
        gScore.TryGetValue(startNode.GetPosition(), out startScore);
        openQueue.Insert(startNode, startScore);


        int nodesSearched = 0;

        //while the open queue is not empty
        while(openQueue.Count() > 0)
        {
            Node currentNode = openQueue.Dequeue();

            nodesSearched++;
            //check if reached goal
            if(currentNode == lookingFor)
            {
                //return the reconstructed path
                return ConstructPath(cameFrom, currentNode);
            }


            //current g score
            float currentGScore = 0;
            gScore.TryGetValue(currentNode.GetPosition(), out currentGScore);

            //for each neighbor of the current node
            for (int i = 0; i < 4; i++)
            {
                Node neighborNode = currentNode.GetNeighbor(i);

                //if the neighbor exists
                if(neighborNode != null)
                {
                    //get g score
                    float neighborGScore = currentGScore + Distance(neighborNode, lookingFor);

                    //see if new g score is better than previous
                    float oldNeighborScore = 0;
                    gScore.TryGetValue(neighborNode.GetPosition(), out oldNeighborScore);
                    oldNeighborScore = oldNeighborScore == 0 ? float.PositiveInfinity : oldNeighborScore;

                    if (neighborGScore < oldNeighborScore)
                    {
                        Vector2 neighborPosition = neighborNode.GetPosition();

                        //set the new came from map
                        if (cameFrom.ContainsKey(neighborPosition)) { cameFrom.Remove(neighborPosition); }
                        cameFrom.Add(neighborPosition, currentNode);

                        //set the new g score
                        if (gScore.ContainsKey(neighborPosition)) { gScore.Remove(neighborPosition); }
                        gScore.Add(neighborPosition, neighborGScore);

                        //if this node isn't queued to be search, queue it
                        if (!openQueue.Contains(neighborNode)) openQueue.Insert(neighborNode, neighborGScore);
                    }
                }
            }
        }
        //couldn't find a path
        return null;
    }


    private Stack<Node> ConstructPath(Dictionary<Vector2, Node> cameFrom, Node current)
    {
        Stack<Node> returnStack = new Stack<Node>();
        HashSet<Vector2> exists = new HashSet<Vector2>();

        returnStack.Push(current);

        while(current != null && cameFrom.ContainsKey(current.GetPosition()))
        {
            cameFrom.TryGetValue(current.GetPosition(), out current);
            returnStack.Push(current);

            if(exists.Contains(current.GetPosition()))
            {
                Debug.Log("ALREADY EXISTS");
            }
            else
            {
                exists.Add(current.GetPosition());
            }
        }



        return returnStack;
    }


    private float Distance(Node currentNode, Node lookingFor)
    {
        return Vector3.Distance(currentNode.GetPosition(), lookingFor.GetPosition());
    }


    /// <summary>
    /// This function Updates the AI's destination
    /// </summary>
    private void UpdateDestination()
    {
        if(path.Count > 0)
        {
            lastNode = destinationNode;
            destinationNode = path.Dequeue();
        }
        else
        {
            Node newDestination;

            lastNode = destinationNode;
            //generate new path
            CreatePath();

            //successful dequeue
            if(path.TryDequeue(out newDestination))
            {
                destinationNode = newDestination;
            }
            //couldn't find a path -- THIS SHOULD NEVER HAPPEN
            else
            {
                Debug.Log("FAILED DEQUEUE");
                InitializeEnemy();
            }
        }
    }

    // ================================================================ //
    // ======================= Public Functions ======================= //
    // ================================================================ //

    public void KnockBackAI(float newKnockBackTime)
    {
        knockBackTime = newKnockBackTime;
    }

    public bool GetIsKnockBacked()
    {
        return knockBackTime > 0;
    }


}
