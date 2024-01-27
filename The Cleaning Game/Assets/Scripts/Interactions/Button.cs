using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] GameObject target;
    public static GameObject buttonPressedNoise { get; set; }
    public void Pressed()
    {
        Activatable activatable = target.GetComponent(typeof(Activatable)) as Activatable;
        activatable.Activate();

        PlayButttonNoise();
    }

    private void PlayButttonNoise()
    {
        Instantiate(buttonPressedNoise, transform.position, transform.rotation);
    }
}
