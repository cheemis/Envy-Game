using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FlatPlayerController : MonoBehaviour
{
    /*
    // ================================================================ //
    // =========================== Variables ========================== //
    // ================================================================ //

    //Grid
    [SerializeField]
    private GraphManager graphManager;

    //movement variables
    [SerializeField]
    private float speed = 10;
    [SerializeField]
    private Vector2Int lastDirection = new Vector2Int(0,0);
    [SerializeField] private Node lastNode;
    [SerializeField] private Node targetNode;



    // ================================================================ //
    // =================== Built In Unity Functions =================== //
    // ================================================================ //


    // Start is called before the first frame update
    void Start()
    {
        //set the initial position of the player
        transform.position = graphManager.GetNodePosition(transform.position);
        lastPosition = transform.position;
        destination = transform.position;
    }
    


    //used for recieving input from the player
    void Update()
    {
        bool pressed = ReadInput();

        //if the player pressed a new direction
        if(pressed)
        {
            UpdateDestination();
        }
    }
    


    //used for updating the position of the player
    private void FixedUpdate()
    {
        MoveCharacter();
    }




    // ================================================================ //
    // ======================= Private Functions ====================== //
    // ================================================================ //


    /// <summary>
    /// This function is a temporary function for recieving input from
    /// the player
    /// </summary>
    /// 
    /// <returns>
    /// A bool if the player changed directions
    /// </returns>
    //dont ask me why i made this overly complicated
    private bool ReadInput()
    {
        //check if new direction pressed
        bool pressed = false;

        //get input
        float hor = Input.GetAxisRaw("Horizontal");
        float vert = Input.GetAxisRaw("Vertical");

        //convert to int
        int horizontal = hor == 0 ? 0 : (int)Mathf.Sign(hor);
        int vertical = vert == 0 ? 0 : (int)Mathf.Sign(vert);
        Vector2 newDirection = new Vector2(horizontal, vertical);

        //if newly held direction for horizontal movement
        if (horizontal != 0 && horizontal != lastDirection.x)
        {
            //newly held direction
            lastDirection = Vector2Int.right * horizontal;
            pressed = true;
        }

        //if newly held direction for vertical movement
        if (vertical != 0 && vertical != lastDirection.y)
        {
            //newly held direction
            lastDirection = Vector2Int.up * vertical;
            pressed = true;
        }

        return pressed;
    }



    /// <summary>
    /// This function moves the player physically. This function should
    /// be called in FixedUpdate()
    /// </summary>
    private void MoveCharacter()
    {
        Vector2 worldSpace = new Vector2(transform.position.x,
                                         transform.position.y);


        float distance = Vector2.Distance(worldSpace, destination);

        transform.position = Vector3.Lerp(worldSpace, destination, speed / (distance * 100)); // MAGIC NUMBER

        if (distance < 0.1f)
        {
            lastPosition = destination;

            UpdateDestination();
        }
    }



    /// <summary>
    /// This function gets the next tile that the player should visit.
    /// This function also checks if the next tile is out of bounds
    /// </summary>
    /// 
    /// <returns>
    /// The world position of a valid tile or Vector3.zero for invalid tiles
    /// </returns>





    /// <summary>
    /// This function Updates the player's destination
    /// </summary>
    private void UpdateDestination()
    {
        //get new tile position
        Vector3 newDestination = GetTilePosition(destination);

        //if the new destination isn't out of bounds
        if (newDestination != Vector3.zero)
        {
            destination = newDestination;
        }
    }



    // ================================================================ //
    // ======================= Public Functions ======================= //
    // ================================================================ //
    */
}
