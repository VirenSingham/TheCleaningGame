using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bucket : MonoBehaviour
{
    [SerializeField] Transform tapCheck;
    [SerializeField] Transform waterLevelTrans;
    [SerializeField] float startWaterLevel = 0f;
    [SerializeField] float lowThreshold = 0f;
    [SerializeField] float highThreshold = 0.6f;
    [SerializeField] float detectionRange = 0f;
    [SerializeField] LayerMask refillLayer;
    [SerializeField] float flowRate = 0.1f;

    RaycastHit hit;

    float waterLevel;

    private void Start()
    {
        waterLevel = startWaterLevel;
    }

    void FixedUpdate()
    {
        changeWaterLevel();
        updateVisualWaterLevel();
    }

    // TODO:
    void updateVisualWaterLevel()
    {
        float x = waterLevelTrans.localPosition.x;
        float z = waterLevelTrans.localPosition.z;

        waterLevelTrans.localPosition = new Vector3(x, waterLevel, z);
    }

    void changeWaterLevel()
    {
        checkUnderTap();
        checkDirtyMop();

        clampWaterLevel();
    }

    /*
     * Clamp water level in case it has changed to be
     * outside given bounds
     */
    void clampWaterLevel()
    {
        if (waterLevel < lowThreshold)
            waterLevel = lowThreshold;

        else if (waterLevel > highThreshold)
            waterLevel = highThreshold;
    }

    //TODO:
    void checkDirtyMop()
    {
    }

    // If under tap increase level by flowrate 
    void checkUnderTap()
    {
        if (Physics.Raycast(new Ray(tapCheck.position, tapCheck.up), out hit, detectionRange, refillLayer))
        {
            if (hit.collider.CompareTag("waterSpout"))
                waterLevel += flowRate;
        }
    }
}
