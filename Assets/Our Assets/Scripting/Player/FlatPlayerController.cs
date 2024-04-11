using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FlatPlayerController : MonoBehaviour
{
    // ================================================================ //
    // =========================== Variables ========================== //
    // ================================================================ //

    //tilemap assets
    [SerializeField]
    private Tilemap tilemap;
    public Sprite outOfBoundsSprite;
    private float tileSize;


    //movement variables
    [SerializeField]
    private float speed = 10;
    [SerializeField]
    private Vector2Int lastDirection = new Vector2Int(0,0);
    [SerializeField] private Vector2 lastPosition = new Vector2(0,0);
    [SerializeField] private Vector2 destination = new Vector2(0,0);



    // ================================================================ //
    // =================== Built In Unity Functions =================== //
    // ================================================================ //


    // Start is called before the first frame update
    void Start()
    {
        //get the size of the tiles in the tilemap
        tileSize = tilemap.cellSize.x;

        //set the initial position of the player
        transform.position = GetTilePosition();
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
    private Vector3 GetTilePosition()
    {
        //get the position of the next tile to visit
        Vector3 newPosition = new Vector3(destination.x + lastDirection.x * tileSize,
                                          destination.y + lastDirection.y * tileSize,
                                          0);

        //get the sprite of the next tile to visit
        Vector3Int cellLocation = tilemap.WorldToCell(newPosition);
        Sprite tileSprite = tilemap.GetSprite(cellLocation);

        //if the tile exists and its not out of bounds
        if(tileSprite != null && tileSprite != outOfBoundsSprite)
        {
            return tilemap.GetCellCenterWorld(cellLocation);
        }

        //else, return empty
        return Vector3.zero;
    }


    /// <summary>
    /// This function Updates the player's destination
    /// </summary>
    private void UpdateDestination()
    {
        //get new tile position
        Vector3 newDestination = GetTilePosition();

        //if the new destination isn't out of bounds
        if (newDestination != Vector3.zero)
        {
            destination = newDestination;
        }
    }



    // ================================================================ //
    // ======================= Public Functions ======================= //
    // ================================================================ //

}
