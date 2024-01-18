using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class RoomManager : MonoBehaviour
{
    [SerializeField] [Tooltip("DEBUG STUFF DON'T TOUCH")] List<GameObject> props;
    [SerializeField] [Tooltip("Number of Deviants allowed in a room")] int maxDeviants;
    [SerializeField] [Tooltip("The number of deviants that will start in a room")]int startingDeviants;

    //the room manager's assigned room's mess
    int messCount;

    //the total mess of every room
    static int totalMessCount;

    int deviantCount;

    [SerializeField] TextMeshPro messDisplay;
    [SerializeField] TextMeshPro totalMessDisplay;

    //code for making sure collision runs
    bool hasInitialised = true;
    bool isTimerRunning;
    
    float timerDuration = 5.0f; //length of timer

    void StartTimer()
    {
        isTimerRunning = true;
    }
    void StopTimer()
    {
        timerDuration = 0;
        isTimerRunning = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        //ensure deviant count is initialised
        deviantCount = 0;
        StartTimer();

        //TEMPORARILY PUTTING UPDATE TEXT HERE
        UpdateMessCount();
    }

    // Update is called once per frame
    void Update()
    {
        //Check if timer is running
        if (isTimerRunning)
        {
            timerDuration -= Time.deltaTime;
            if (timerDuration <= 0)
            {
                StopTimer();
            }
        }
        else
        {
            if (hasInitialised)
            {
                InitialiseRoom();
                hasInitialised = false;
            }
        }
    }

    //Randomise what object is a deviant.
    void InitialiseRoom()
    {
        //sort through our list of objects and randomly pick an object to be a deviant.
        //There is definitely a better way to do this that would be more random. But we're going with this for now
        for (int i = 0; i < props.Count; i++)
        {
            //if we don't have enough many deviants
            if (deviantCount < startingDeviants) 
            {
                RandomiseDeviant(props[i]);
            }
            //check to make sure we have enough deviants
            else
            {
                int existingDeviants = 0;
                for (int f = 0; f < props.Count; f++)
                {
                    if (props[f].GetComponent<ObjectBehaviour>().isDeviant == true)
                    {
                        existingDeviants++;
                    }
                }
                if (existingDeviants != startingDeviants)
                {
                    i = 0;
                }
            }
        }
    }

    void SetDeviant()
    {

    }

    void TriggerDeviantAction()
    {

    }

    void RandomiseDeviant(GameObject prop)
    {
        //coin flip to see if it will be a deviant or not (will be 0 or 1)
        int rnd = UnityEngine.Random.Range(0, 2);
        //check if it's a 1, if so it's a deviant
        if (rnd == 1)
        {
            prop.GetComponent<ObjectBehaviour>().isDeviant = true;
            deviantCount++;
            Debug.Log("IS DEVIANT");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //filter objects by tag and add them to the prop list
        //or filter by if they have the object script.

        if (other.gameObject.tag == "object") 
        {
            props.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //filter objects by tag and add them to the prop list
        //or filter by if they have the object script.
        if (other.gameObject.tag == "object")
        {
            props.Remove(other.gameObject);
        }
    }

    void UpdateMessCount()
    {
        messDisplay.text = messCount.ToString();
        totalMessDisplay.text = totalMessCount.ToString();
    }
}
