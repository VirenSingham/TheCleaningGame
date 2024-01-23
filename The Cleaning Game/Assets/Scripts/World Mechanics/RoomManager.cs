using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class RoomManager : MonoBehaviour
{
    [Tooltip("DEBUG STUFF DON'T TOUCH")] public List<GameObject>props;
    [SerializeField] [Tooltip("DEBUG STUFF DON'T TOUCH")] List<GameObject> mess;
    [Tooltip("Number of Deviants allowed in a room")] public int maxDeviants;
    [SerializeField] [Tooltip("The number of deviants that will start in a room")]int startingDeviants;

    //the room manager's assigned room's mess objects. So this is the count of how many mess objects are in the room
    int messCount;

    //messModifier is the added mess via objects that are toppled over for example. So once a object is toppled it will add to this and then minus once set upright.
    //this value is added to messCount after messCount has been set properly. So when an object causes a mess that is not a physical gameobject. So not a mess puddle. 
    //Add to the messModifier instead of messCount. This was we can track both independantly but still combine there values.
    [HideInInspector] public int messModifier;

    [SerializeField] LayerMask messLayer;

    //the total mess of every room
    static int totalMessCount;

    [HideInInspector] public int deviantCount;

    [HideInInspector] public bool isPlayerInRoom;

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
        UpdateMessCount();
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
        int rnd = UnityEngine.Random.Range(0, 3);
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
            if (other.gameObject.GetComponent<ObjectBehaviour>().isMessy == true)
            {
                messModifier++;
            }
        }
        else if (other.gameObject.tag == "Player")
        {
            isPlayerInRoom = true;
        }
        else if (other.gameObject.layer == messLayer) 
        { 
            mess.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //filter objects by tag and add them to the prop list
        //or filter by if they have the object script.
        if (other.gameObject.tag == "object")
        {
            props.Remove(other.gameObject);

            if (other.gameObject.GetComponent<ObjectBehaviour>().isMessy == true)
            {
                messModifier--;
            }
        }
        else if (other.gameObject.tag == "Player")
        {
            isPlayerInRoom = false;
        }
        else if (other.gameObject.layer == messLayer)
        {
            mess.Remove(other.gameObject);
        }
    }

    void UpdateMessCount()
    {
        messCount = mess.Count;
        messCount += messModifier;

        messDisplay.text = messCount.ToString();
        totalMessDisplay.text = totalMessCount.ToString();
    }
}
