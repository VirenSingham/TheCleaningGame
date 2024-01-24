using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    [SerializeField] [Range(0, 100)] float charge = 100;
    [SerializeField] float slotDetectionRange = 1f;
    [SerializeField] LayerMask refillLayer;

    bool isSlotted = false;
    string batSlotTag = "BatSlot";
    int maxChecks = 100;

    private void Update()
    {
        if (!isSlotted)
            CheckForBatterySlots();
    }

    private void CheckForBatterySlots()
    {
        Collider[] refills = new Collider[maxChecks];

        // Find All Colliders close to battery
        int refillsFound = Physics.OverlapSphereNonAlloc(transform.position, slotDetectionRange, refills, refillLayer);

        // Loop through refills for battery slot, attatch if one is found
        for (int i = 0; i < refillsFound; i++)
        {
            if (refills[i].tag.Equals(batSlotTag))
            {
                AttatchToSlot(refills[i].gameObject);
                return;
            }
        }
    }

    public float getCharge()
    {
        return charge;
    }

    public void addCharge(float charge)
    {
        this.charge += charge;
    }

    public void setTransform(Transform t)
    {
        transform.position = t.position;
        transform.rotation = t.rotation;
    }

    private void AttatchToSlot(GameObject batterySlotObject)
    {
        BatterySlot batSlot = batterySlotObject.GetComponent(typeof(BatterySlot)) as BatterySlot;
        if (!batSlot.isHoldingBattery() && batSlot.isReadyToCharge())
        {
            batSlot.AttatchBattery(this);
            isSlotted = true;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    public void DetatchFromSlot()
    {
        Debug.Log("Detatched");
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        isSlotted = false;
    }
}
