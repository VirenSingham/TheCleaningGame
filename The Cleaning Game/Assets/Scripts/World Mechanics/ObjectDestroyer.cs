using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class ObjectDestroyer : MonoBehaviour
{
    [SerializeField] TextMeshPro payDisplay;

    [SerializeField] [Tooltip("what objects that have no value will default to")] int defaultValue;

    [SerializeField][Tooltip("where the player goes if they fall in the hole")] Transform teleportPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //filter objects by tag and add them to the prop list
        //or filter by if they have the object script.

        if (other.gameObject.tag == "object")
        {
            //get our current pay from the pay display.
            int currentMoney = int.Parse(payDisplay.text);

            //minus the value of the object destroyed form our current pay using the default value (used when an object has no set value)
            currentMoney -= other.GetComponent<ObjectBehaviour>().value;
            payDisplay.text = currentMoney.ToString();
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag != "Player")
        {
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "Player")
        {
            //if we want a death state for the player killing themselves it would go here
            other.gameObject.transform.SetPositionAndRotation(teleportPoint.position, teleportPoint.rotation);
        }
    }
}
