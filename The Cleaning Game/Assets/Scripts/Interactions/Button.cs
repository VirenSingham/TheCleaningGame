using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] GameObject target;

    public void Pressed()
    {
        Activatable activatable = target.GetComponent(typeof(Activatable)) as Activatable;
        activatable.Activate();
    }
}
