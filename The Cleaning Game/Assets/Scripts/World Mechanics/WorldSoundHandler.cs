using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSoundHandler : MonoBehaviour
{
    [SerializeField] GameObject buttonPressedNoise;

    private void Awake()
    {
        Button.buttonPressedNoise = buttonPressedNoise;
    }
}
