using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
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
    private Vector2Int direction = new Vector2Int(0, 0);

    //Pathing Variables
    public Queue<Node> path = new Queue<Node>();


    //testing variables
    public int depth;
    private HashSet<Vector2> visited = new HashSet<Vector2>();




    // Start is called before the first frame update
    void Start()
    {
        InitializeEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        MoveCharacter();
    }

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

        transform.position = Vector3.Lerp(worldSpace, destination, speed / (distance * 100)); // MAGIC NUMBER

        if (distance < 0.1f)
        {
            UpdateDestination();
        }
    }


    private void CreatePath()
    {
        //choose a random target in graph
        Node newTarget = graphManager.GetRandomNode();

        Stack<Node> stack = DepthFirstSearch(destinationNode, newTarget);

        //couldn't find path
        if (stack == null)
        {
            return;
        }

        Debug.Log("found a path that is " +  stack.Count + " nodes long");

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

            //generate new path
            visited.Clear();
            CreatePath();

            //successful dequeue
            if(path.TryDequeue(out newDestination))
            {
                destinationNode = path.Dequeue();
            }
            //couldn't find a path -- THIS SHOULD NEVER HAPPEN
            else
            {
                InitializeEnemy();
            }

            
        }
    }

}
