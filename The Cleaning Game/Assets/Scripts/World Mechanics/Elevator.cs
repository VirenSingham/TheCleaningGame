using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    //references to the top and bottom of the elevator
    [Header("Elevation raycast points")]
    [SerializeField][Tooltip("Refence for the elevator roof")] GameObject roofRef;
    [SerializeField] [Tooltip("Refence for the elevator floor")] GameObject floorRef;

    [Header("Door Object References")]
    [SerializeField][Tooltip("Refence for the right elevator door")] GameObject DoorRightRef;
    [SerializeField][Tooltip("Refence for the left elevator door")] GameObject DoorLeftRef;

    [Header("Door Raycast Points")]
    [SerializeField][Tooltip("Refence for the left elevator door")] GameObject DoorLeftOpenRef;
    [SerializeField][Tooltip("Refence for the left elevator door")] GameObject DoorLeftCloseRef;

    [Header("Child Ref")]
    [SerializeField] GameObject importantStuffHolder;
    [SerializeField] GameObject playerRef;

    //Elevator behavior variables
    [SerializeField] [Tooltip("Speed of the Elevators Movement")] 
    float elevationSpeed;

    [SerializeField][Tooltip("Whether the elevator is starting on the bottom floor or not")] 
    private bool isStartingBottomFloor;

    //private elevator variables
    private bool isBottomFloor;
    private bool isMovingUp;
    private bool isMoving;

    [SerializeField] LayerMask doorClose;

    //private elevator doors variables
    //are our doors closed?
    bool isClosed;
    [SerializeField] [Tooltip("How fast the elevator doors close")] float closeSpeed;

    // Start is called before the first frame update
    void Start()
    {
        isMoving = false;

        //set our values based on whether we are starting on the bottom floor
        if (isStartingBottomFloor)
        {
            isBottomFloor = true;
        }
        else
        {
            isBottomFloor = false;
        }    
    }

    private void FixedUpdate()
    {
        //update the state of our doors
        UpdateDoors();
    }

    // Update is called once per frame
    void Update()
    {
        //debug elevator activation. Finished version will trigger this function on raycast player interaction.
        if (Input.GetKeyDown(KeyCode.Space)) 
        { 
            ButtonPressed();
        }

        //if the Elevator is moving
        if (isMoving) 
        { 
            //if the elevator is going to the top floor
            if (isMovingUp) 
            {
                //move the elevator upwards, modified by our elevation speed
                transform.Translate(transform.up * elevationSpeed * Time.deltaTime);

                //if the elevator roof touches something, stop
                if (Physics.Raycast(roofRef.transform.position, roofRef.transform.up, 0.1f))
                {
                    isMoving = false;
                    isBottomFloor = false;
                    //remove player from parent and return it to important stuff
                    //playerRef.transform.SetParent(importantStuffHolder.transform, true);
                }
            }
            //if the elevator is going to the bottom floor (closet)
            else if (!isMovingUp)
            {
                //move the elevator downwards, modified by our elevation speed
                transform.Translate((transform.up * -1) * elevationSpeed * Time.deltaTime);

                //if the elevator floor touches something, stop
                if (Physics.Raycast(floorRef.transform.position, floorRef.transform.up * -1, 0.1f))
                {
                    isMoving = false;
                    isBottomFloor = true;
                    //remove player from parent and return it to important stuff
                    //playerRef.transform.SetParent(importantStuffHolder.transform, true);
                }
            }
        }
    }

    void ButtonPressed()
    {
        //if we aren't moving
        if (!isMoving)
        {
            //if we are currently on the bottom floor
            if (isBottomFloor)
            {
                isMovingUp = true;
                isMoving = true;
            }
            //if we are currently on the top floor
            else
            {
                isMovingUp = false;
                isMoving = true;
            }
            //set the player to be a child of elevator
            //playerRef.transform.SetParent(gameObject.transform, true);
        }
    }

    void UpdateDoors()
    {
        //if we are moving close the elevator doors
        if (isMoving)
        {
            isClosed = true;
            
            // close doors
            if (!Physics.Raycast(DoorLeftCloseRef.transform.position, DoorLeftCloseRef.transform.right, 0.15f, doorClose))
            {
                
                DoorLeftRef.transform.Translate(transform.right * closeSpeed * Time.deltaTime);
                DoorRightRef.transform.Translate((transform.right * -1) * closeSpeed * Time.deltaTime);
            }
        }
        //if we are not moving open the elevator doors
        else 
        { 
            isClosed = false;
            //open doors
            if (!Physics.Raycast(DoorLeftOpenRef.transform.position, DoorLeftOpenRef.transform.right * -1 ,0.15f, doorClose))
            {
                DoorLeftRef.transform.Translate((transform.right * -1) * closeSpeed * Time.deltaTime);
                DoorRightRef.transform.Translate(transform.right * closeSpeed * Time.deltaTime);
            }
        }
    }
}
