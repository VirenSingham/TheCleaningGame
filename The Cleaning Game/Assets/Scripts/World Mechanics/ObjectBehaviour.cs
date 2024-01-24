using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
//using static Unity.VisualScripting.Member;

public class ObjectBehaviour : MonoBehaviour
{
    [HideInInspector] public Rigidbody rb;
    public LayerMask ground;

    //deviant check values
    public bool isDeviant;
    bool didDeviantAct;
    bool canDoDeviousAct;

    public int value;

    bool isDirty;
    bool isDeviantDirty;

    [HideInInspector] public bool isMessy;

    //timer values
    bool isTimerRunning;
    float timerDuration;

    RoomManager roomManager;
    
    GameObject purpleMess;
    GameObject deviantParticles;
    //Deviant Acts Values

    int deviantActCounter = 0; //tracks how many devious acts have succeeded
    
    int spreadCounter = 0; //tracks how many devious acts we've done. If we spread this will reset to zero
    
    int spreadDeviancyThreshold; //The amount of devious acts that must be commited before the object can spread deviancy
    
    int revealSelfThreshold; //The amount of devious acts that will cause the Deviant to reveal itself (coating itself in purple muck)
    
    bool hasRevealedSelf = false; //has this object revealed itself as a deviant?

    [SerializeField] float devianceRange;
    [SerializeField] float throwStrength;

    // Start is called before the first frame update
    void Start()
    {
        didDeviantAct = false;
        canDoDeviousAct = false;

        //----------------------change deviancy thresholds here----------------------------------

        //DEFAULT IS 4 SPREAD DEVIANCY THRESHOLD AND 6 REVEAL.
        spreadDeviancyThreshold = 3;
        revealSelfThreshold = 5;

        if (devianceRange == 0)
        { 
            devianceRange = 0.5f;
        }

        //DEBUG DELETE LATER
        devianceRange = 3;

        if (throwStrength == 0) 
        { 
            throwStrength = 15;
        }

        rb = gameObject.GetComponent<Rigidbody>();

        purpleMess = GameObject.Find("purple_mess");
        deviantParticles = GameObject.Find("DeviantPEref");

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

        Vector3 lastTransform = transform.position;

        CheckMessStatus();
    }

    void TryDeviousAction()
    {
        //has the player entered this room before? Are they currently in the room?
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

        //this check is also made in spread deviancy but I can't be bothered to refactor whilst we are time pressured. Not worth the time waste.
        int rnd;
        if (spreadCounter >= spreadDeviancyThreshold)
        {
            rnd = UnityEngine.Random.Range(0, 4);
        }
        else
        {
            rnd = UnityEngine.Random.Range(1, 4);
        }


        //failed deviant acts do nothing and add nothing to the act count.
        switch (rnd) 
        {
            //spread deviancy to another object, Deviancy isn't considered a mess
            //thus we don't modify the messModifier
            case 0: 
                {
                    SpreadDeviancy();
                    Debug.Log("Tried to Spread Self");
                    break; 
                }

            //Throw self around to another position, thrown objects will automatically check if they are toppled over and add to mess.
            //thus we don't modify the messModifier
            case 1: 
                {
                    DeviantSelfThrow();
                    Debug.Log("Threw Self");
                    break;
                }
            //create a purple mess puddle nearby the roomManager automatically adds mess objects to the messCount,
            //thus we don't modify the messModifier
            case 2:
                {
                    MakeMess();
                    Debug.Log("Made a Mess");
                    break;
                }
            case 3:
                {
                    ThrowOtherObject();
                    Debug.Log("Threw another Object");
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
        Mathf.Abs(randomDirection.y);

        rb.AddForce(randomDirection * (throwStrength + (rb.mass * 2)), ForceMode.Impulse);
        deviantActCounter++;
        spreadCounter++;
    }

    //Deviant Act: Create a puddle of mess nearby itself. It can only make purple messes.
    void MakeMess()
    {
        //loop through the room and find objects that are close and not deviants
        for (int i = 0; i < roomManager.props.Count; i++)
        {
            //check if we are close to the object in the room and that it isn't already deviant
            if (Vector3.Distance(gameObject.transform.position, roomManager.props[i].transform.position) <= devianceRange)
            {
                //raycast for the ground so we can set the height of our spawn location properly.
                RaycastHit hit;
                Physics.Raycast(roomManager.gameObject.transform.position, roomManager.gameObject.transform.up * -1, out hit, 50f, ground);

                //get the midpoint between the objects
                Vector3 midPoint = (gameObject.transform.position + roomManager.gameObject.transform.position) / 2;

                //make sure it's height is the same as the floor
                Vector3 transformModded = midPoint;
                transformModded.y = hit.transform.position.y;

                Instantiate(purpleMess, transformModded, purpleMess.transform.rotation);

                deviantActCounter++;

                //end our loop by setting i to the size of our list thus ending the loop
                i = roomManager.props.Count;
            }
        }
    }

    void ThrowOtherObject()
    {
        //loop through the room and find objects that are close
        for (int i = 0; i < roomManager.props.Count; i++)
        {
            //check if we are close to the object in the room
            if (Vector3.Distance(gameObject.transform.position, roomManager.props[i].transform.position) <= devianceRange && gameObject != roomManager.props[i])
            {
                //get a random point on a sphere to use as a random direction. 
                Vector3 randomDirection = Random.onUnitSphere;
                Mathf.Abs(randomDirection.y);

                //launch the object based on the other objects mass
                roomManager.props[i].GetComponent<Rigidbody>().AddForce(randomDirection * (throwStrength + (roomManager.props[i].GetComponent<Rigidbody>().mass * 2)), ForceMode.Impulse);
                Debug.Log("Threw: " + roomManager.props[i].name);

                //end our loop by setting i to the size of our list thus ending the loop
                i = roomManager.props.Count;
                spreadCounter++;
                deviantActCounter++;
            }
        }
    }
    //check if we are toppled and messy, then pass it back to the RoomManager
    void CheckMessStatus()
    {
        //double check this logic just in case.
        //if we are toppled over, check if we are standing now.
            Vector3 gameobjectTransformModY = gameObject.transform.position;
            gameobjectTransformModY.y += 5f;

        if (isMessy == true && roomManager.props.Contains(gameObject))
        {
            //raycast down to check for ground. If it hits the ground we are standing now
            if (Physics.Raycast(gameobjectTransformModY, gameObject.transform.up * -1, 50f, ground))
            {
                //adjust messModifier appropriately, and set it to not be messy
                roomManager.messModifier--;
                isMessy = false;
            }
        }
        //if we aren't toppled over, check if we are no longer standing upright
        if (isMessy == false && roomManager.props.Contains(gameObject))
        {
            //raycast down to check for ground. if it doesn't hit anything, we are not standing upright.
            if (!Physics.Raycast(gameobjectTransformModY, gameObject.transform.up * -1, 50f, ground))
            {
                //adjust messModifier appropriately, and set it to not be messy
                roomManager.messModifier++;
                isMessy = true;
            }
        }
    }

    void RevealDeviancy()
    {
        //add particle effect to deviant object based off reference gotten during start
        GameObject particleEffect = Instantiate(deviantParticles,gameObject.transform);
        particleEffect.transform.SetParent(gameObject.transform, true);
        particleEffect.transform.SetPositionAndRotation(gameObject.transform.position, gameObject.transform.rotation);
        Debug.Log("revealed self");
    }

    //Timer functions for randomising deviant activity.
    void StartTimer()
    {
        float rnd = Random.Range(3, 16);
        timerDuration = rnd;
        isTimerRunning = true;
    }
    void StopTimer()
    {
        timerDuration = 0;
        isTimerRunning = false;
    }
}
