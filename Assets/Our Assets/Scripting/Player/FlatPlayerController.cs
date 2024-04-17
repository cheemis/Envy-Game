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

    //Grid
    [SerializeField]
    private GraphManager graphManager;
    private Node lastNode;
    private Node targetNode;

    //movement variables
    [SerializeField]
    private float speed = 10;
    [SerializeField]
    private Vector2Int currentDirection = new Vector2Int(0,0);
    [SerializeField]
    private Vector2Int lastDirection = new Vector2Int(0,0);
    [SerializeField]
    private float knockback = -2;
    private float knockBackTime = 0;




    // ================================================================ //
    // =================== Built In Unity Functions =================== //
    // ================================================================ //


    // Start is called before the first frame update
    void Start()
    {
        //set the initial position of the player
        Vector3 tilePosition = graphManager.GetNodePosition(transform.position);
        transform.position = tilePosition;
        lastNode = graphManager.GetNode(tilePosition);
        targetNode = lastNode;

        //update the player speed/knock back by checking the game manager
        speed = GameManager.Instance.GetPlayerSpeed();
        knockback = GameManager.Instance.GetPlayerKnockBack();
    }
    


    //used for recieving input from the player
    void Update()
    {
        bool pressed = ReadInput();

        //if the player pressed a new direction
        if(pressed)
        {
            //UpdateDestination();
        }

        if (knockBackTime <= 0)
        {
            MoveCharacter();
        }
        else
        {
            knockBackTime -= Time.deltaTime;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collision 2D with " + collision.gameObject.name);

        if (collision.tag == "Enemy Player")
        {
            EnemyAI enemy = collision.GetComponent<EnemyAI>();

            Debug.Log("bumped into enemy!");

            //if no one is already knocked back
            if (!GetKnockedBack() && !enemy.GetIsKnockBacked())
            {
                //apply slowing
                if (knockback < 0)
                {
                    //knock back player
                    knockBackTime = -knockback;
                    Debug.Log("knocked back player");
                }
                else
                {
                    //knock back enemy
                    enemy.KnockBackAI(knockback);
                    Debug.Log("knocked back enemy");
                }
            }
        }
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

        //if newly held direction for horizontal movement
        if (horizontal != 0 && horizontal != currentDirection.x)
        {
            //newly held direction
            currentDirection = Vector2Int.right * horizontal;
            pressed = true;
        }

        //if newly held direction for vertical movement
        if (vertical != 0 && vertical != currentDirection.y)
        {
            //newly held direction
            currentDirection = Vector2Int.up * vertical;
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

        Vector3 destination = targetNode.GetPosition();

        float distance = Vector2.Distance(worldSpace, destination);

        transform.position = Vector3.Lerp(worldSpace, destination, (speed / (distance * 10)) * Time.deltaTime); // MAGIC NUMBER

        if (distance < 0.1f)
        {
            // delete the pellet on that node
            graphManager.DeleteThePelletOnNode(destination, true);

            UpdateDestination();
        }
    }



    /// <summary>
    /// This function Updates the player's destination
    /// </summary>
    private void UpdateDestination()
    {
        Node newTarget = targetNode.GetNeighbor(currentDirection);

        //Debug.Log("destination change: Old: " + targetNode.GetPosition() + ", New: " + (newTarget == null ? "Null" : newTarget.GetPosition()));
        lastNode = targetNode;

        //if the new direction worked
        if (newTarget != null)
        {
            lastDirection = currentDirection;
            targetNode = targetNode.GetNeighbor(currentDirection);
            return;
        }


        //use the old direction
        newTarget = targetNode.GetNeighbor(lastDirection);
        if (newTarget != null)
        {
            targetNode = targetNode.GetNeighbor(lastDirection);
            return;
        }

    }



    // ================================================================ //
    // ======================= Public Functions ======================= //
    // ================================================================ //


    /// <summary>
    /// This function adds speed to the player's base speed
    /// </summary>
    public void AddPlayerSpeed(float addSpeed)
    {
        speed += addSpeed;
    }

    public bool GetKnockedBack()
    {
        return knockBackTime > 0;
    }


}
