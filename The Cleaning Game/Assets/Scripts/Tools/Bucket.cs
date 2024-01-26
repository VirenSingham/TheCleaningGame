using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bucket : MonoBehaviour
{
    [SerializeField] Transform tapCheck;
    [SerializeField] Transform waterLevelTrans;
    [SerializeField] LayerMask refillLayer;
    [SerializeField] ParticleSystem BucketSplash;
    [SerializeField] float startWaterLevel = 0f;
    [SerializeField] float lowThreshold = 0f;
    [SerializeField] float highThreshold = 0.6f;
    [SerializeField] float detectionRange = 0f;
    [SerializeField] float flowRate = 0.1f;
    [SerializeField] float waterLevel;

    RaycastHit hit;

    private void Start()
    {
        waterLevel = startWaterLevel;
    }

    void FixedUpdate()
    {
        changeWaterLevel();
        updateVisualWaterLevel();

    }

    /*
     * transforms the water to be at the appropriate height
     * disables the water object if level = 0
     */
    void updateVisualWaterLevel()
    {
        float x = waterLevelTrans.localPosition.x;
        float z = waterLevelTrans.localPosition.z;

        waterLevelTrans.localPosition = new Vector3(x, waterLevel, z);

        if (waterLevel == 0)
            waterLevelTrans.gameObject.SetActive(false);
        else
            waterLevelTrans.gameObject.SetActive(true);
    }

    void changeWaterLevel()
    {
        checkUnderTap();

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

    // If under tap increase level by flowrate 
    void checkUnderTap()
    {
        if (Physics.Raycast(new Ray(tapCheck.position, tapCheck.up), out hit, detectionRange, refillLayer))
        {
            if (hit.collider.CompareTag("waterSpout") && waterLevel < highThreshold)
            {
                Activatable spout = hit.collider.GetComponent(typeof(Activatable)) as Activatable;
                spout.Activate();

                waterLevel += flowRate;
            }
        }
    }

    public float getWaterLevel()
    {
        return waterLevel;
    }

    public void setWaterLevel(float waterLevel)
    {
        this.waterLevel = waterLevel;
    }

    public float getMaxLevel()
    {
        return highThreshold;
    }

    public void MakeSplash()
    {
        BucketSplash.Play();
    }
}
