using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectBehaviour : MonoBehaviour
{
    public bool isDeviant;
    bool didDeviantAct;
    bool canDoDeviousAct;

    public int value;

    bool isDirty;
    bool isDeviantDirty;

    //timer values
    bool isTimerRunning;
    float timerDuration;

    RoomManager roomManager;
    

    // Start is called before the first frame update
    void Start()
    {
        didDeviantAct = false;
        canDoDeviousAct = false;

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
