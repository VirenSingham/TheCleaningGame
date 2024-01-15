using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ParentManager : MonoBehaviour
{
    [SerializeField] GameObject important;
    [SerializeField] GameObject room1;
    [SerializeField] GameObject room2;
    [SerializeField] GameObject room3;
    [SerializeField] GameObject room4;

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
        other.gameObject.transform.SetParent(transform, true);
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.tag == "Player")
        {
            other.gameObject.transform.SetParent(important.transform, true);
        }
        else if (other.tag == "Room1")
        {
            other.gameObject.transform.SetParent (room1.transform, true);
        }
        else if (other.tag == "Room2")
        {
            other.gameObject.transform.SetParent(room2.transform, true);
        }
        else if (other.tag == "Room3")
        {
            other.gameObject.transform.SetParent(room3.transform, true);
        }
        else if (other.tag == "Room4")
        {
            other.gameObject.transform.SetParent(room4.transform, true);
        }
        else
        {
            other.gameObject.transform.SetParent(null, true);
        }
    }
}
