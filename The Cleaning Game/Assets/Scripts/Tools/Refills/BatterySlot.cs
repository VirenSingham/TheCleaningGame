using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatterySlot : MonoBehaviour, Activatable
{
    [SerializeField] Transform batteryPos;
    [SerializeField] float deactiveTimeLimit;

    Battery attatchedBattery = null;
    bool isHoldingBat = false;
    float deactiveTime = 0;

    private void Update()
    {
        if (isHoldingBat & isReadyToCharge())
            attatchedBattery.setTransform(batteryPos);
    }

    /*********** Button Press ************/
    public void Activate()
    {
        if (isHoldingBat)
            EjectBattery();
    }

    private void EjectBattery()
    {
        attatchedBattery.DetatchFromSlot();
        attatchedBattery = null;
        isHoldingBat = false;
        deactiveTime = deactiveTimeLimit;
    }

    /*********** Slot Functionality ************/
    public void AttatchBattery(Battery battery)
    {
        attatchedBattery = battery;
        isHoldingBat = true;
    }

    public bool isHoldingBattery()
    {
        return isHoldingBat;
    }

    protected void ChargeAttatchedBat(float charge)
    {
        if (isHoldingBattery())
            attatchedBattery.addCharge(charge);
    }

    public float GetBatteryCharge()
    {
        if (isHoldingBattery())
            return attatchedBattery.getCharge();
        
        return 0;
    }

    public bool isReadyToCharge()
    {
        deactiveTime -= Time.deltaTime;

        if (deactiveTime <= 0)
            return true;

        return false;
    }
}
