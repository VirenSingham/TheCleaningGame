using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectBehaviour : MonoBehaviour
{
    Rigidbody rb;

    //deviant check values
    public bool isDeviant;
    bool didDeviantAct;
    bool canDoDeviousAct;

    public int value;

    bool isDirty;
    int messValue;
    bool isDeviantDirty;

    //timer values
    bool isTimerRunning;
    float timerDuration;

    RoomManager roomManager;

    //Deviant Acts Values
    //tracks how many devious acts have succeeded
    int deviantActCounter = 0;

    //tracks how many devious acts we've done. If we spread this will reset to zero
    int spreadCounter = 0;

    //The amount of devious acts that must be commited before the object can spread deviancy
    int spreadDeviancyThreshold;

    //The amount of devious acts that will cause the Deviant to reveal itself (coating itself in purple muck)
    int revealSelfThreshold;

    //has this object revealed itself as a deviant?
    bool hasRevealedSelf = false;

    [SerializeField] float devianceRange;
    

    // Start is called before the first frame update
    void Start()
    {
        didDeviantAct = false;
        canDoDeviousAct = false;

        //----------------------change deviancy thresholds here----------------------------------

        //DEFAULT IS 4 SPREAD DEVIANCY THRESHOLD AND 6 REVEAL. SET TO 1 FOR DEBUG
        spreadDeviancyThreshold = 4;
        revealSelfThreshold = 6;

        if (devianceRange == 0)
        { 
            devianceRange = 5;
        }

        rb = gameObject.GetComponent<Rigidbody>();

        //create an object that is usedto store references so it can find the RoomManager within the hierarchy. Then set it to be the object attached to the script 
        GameObject roomManagerSearcher = gameObject;

        //while the RoomManager has not been set
        while (roomManager == null)
        {
            //set our searcher to the parent of itself
            roomManagerSearcher = roomManagerSearcher.transform.parent.gameObject;

            //check if the searcher has found the room parent (room parent is an empty container object that sorts objects into their rooms e.g. Room 1, Room 2)
            if (roomManagerSearcher.gameObject.tag == "Room1")
            {
                //find the room manager within the Room Parent.
                roomManager = roomManagerSearcher.GetComponentInChildren<RoomManager>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if the object is deviant
        if (isDeviant)
        {
            TryDeviousAction();
        }
    }

    void TryDeviousAction()
    {
        //has the player entered this room before?
        if (canDoDeviousAct)
        {
            //check if the player is in the room
            if (roomManager.isPlayerInRoom == false)
            {
                //if they are in the room start a timer set to a random duration.
                if (didDeviantAct == false && isTimerRunning == false)
                {
                    StartTimer();
                }
                //if the timer is running and the player isn't in the room still
                if (isTimerRunning == true)
                {
                    if (roomManager.isPlayerInRoom == false)
                    {
                        //decrement timer
                        timerDuration -= Time.deltaTime;
                        //check if timer has finished
                        if (timerDuration <= 0)
                        {
                            //if timer finishes commit a devious act
                            StopTimer();
                            CommitDeviantAct();
                        }
                    }
                    //if the player has entered the room again before the timer ended do nothing and stop timer. 
                    else if (roomManager.isPlayerInRoom == true)
                    {
                        StopTimer();
                    }
                }
            }
            // if the player is in the room and we did a devious act we can reset the cooldown on devious actions
            if (roomManager.isPlayerInRoom == true && didDeviantAct == true)
            {
                didDeviantAct = false;
            }
        }
        //if the player hasn't entered the room, check if they've entered yet
        else
        {
            if (roomManager.isPlayerInRoom == true)
            {
                canDoDeviousAct = true;
            }
        }
        
    }

    //runs behaviour for commiting Deviant Actions
    void CommitDeviantAct()
    {
        didDeviantAct = true;

        //debug devious activity. tells me it's done something devious
        Debug.Log("Did something Devious >:3");

        //generate what devious act to do
        //this is hard coded to be the amount of devious acts that can be done. So you'll need to change this as we add more devious acts.
        //the right number must be the same amount as the amount of devious acts that can be done. Although the right number is not inclusive.
        //the right number being the same as the amount of acts still works because we count zero. So if the right number is 3
        //We have three acts because 0,1,2. are 3 numbers.
        int rnd = UnityEngine.Random.Range(0, 3);

        //failed deviant acts do nothing and add nothing to the act count.
        switch (rnd) 
        { 
            //spread deviancy to another object
            case 0: 
                {
                    SpreadDeviancy();
                    Debug.Log("Tried to Spread Self");
                    break; 
                }

            //Throw self around to another position. 
            case 1: 
                {
                    DeviantSelfThrow();
                    Debug.Log("Threw Self");
                    break;
                }
            //create a purple mess puddle nearby
            case 2:
                {
                    MakeMess();
                    Debug.Log("Made a Mess");
                    break;
                }
        }

        //check if the deviant object should reveal it's deviancy
        if (deviantActCounter >= revealSelfThreshold && hasRevealedSelf == false)
        {
            RevealDeviancy();
        }
    }

    //Deviant Act: Make another object a deviant
    void SpreadDeviancy()
    {
        //check if we have done enough devious acts we can spread.
        if (spreadCounter >= spreadDeviancyThreshold)
        {
            //check if we don't have too many deviants in the room
            if (roomManager.deviantCount < roomManager.maxDeviants)
            {
                //loop through the room and find objects that are close and not deviants
                for (int i = 0; i < roomManager.props.Count; i++) 
                { 
                    //check if we are close to the object in the room and that it isn't already deviant
                    if (Vector3.Distance(gameObject.transform.position, roomManager.props[i].transform.position) <= devianceRange && roomManager.props[i].GetComponent<ObjectBehaviour>().isDeviant == false)
                    {
                        //make the object deviant
                        roomManager.props[i].GetComponent<ObjectBehaviour>().isDeviant = true;
                        //end our loop by setting i to the size of our list thus ending the loop
                        i = roomManager.props.Count;

                        Debug.Log("Succesfully Spread Deviancy");

                        spreadCounter = 0;
                        deviantActCounter++;
                    }
                }
            }
        }
    }

    //Deviant Act: launch self in a random direction
    void DeviantSelfThrow()
    {
        Vector3 randomDirection = Random.onUnitSphere;
        rb.AddForce(randomDirection * (7 + rb.mass) , ForceMode.Impulse);
        deviantActCounter++;
        spreadCounter++;
    }

    //Deviant Act: Create a puddle of mess nearby itself. It can only make purple messes.
    void MakeMess()
    {

    }

    //check if we are toppled and messy, then pass it back to the RoomManager
    void CheckMessStatus()
    {
        //mess value is the value that will be added to the RoomManager when calculating the mess of objects.
        //keep in mind mathematically this may have issues, such as if an object is messy thus a value of one. Then we fix it and make it a value of zero.
        //This will mean that the initial mess that was added will still remain and thus make it impossible to fix. Thus we need a method of checking
        //if an object was messy and now isn't and thus once we should minus from the mess counter whilst leaving the mess value at 0. Hopefully future me understands this cause it's
        //VERY IMPORTANT.
    }

    void RevealDeviancy()
    {
        Debug.Log("Showed I'm a Deviant");
    }

    //Timer functions for randomising deviant activity.
    void StartTimer()
    {
        float rnd = Random.Range(3, 21);
        timerDuration = rnd;
        isTimerRunning = true;
    }
    void StopTimer()
    {
        timerDuration = 0;
        isTimerRunning = false;
    }
}
