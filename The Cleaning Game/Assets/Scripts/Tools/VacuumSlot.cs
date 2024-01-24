using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumSlot : BatterySlot
{
    [SerializeField] float batteryConsumptionRate;
    [SerializeField] GameObject vacuumObject;

    Vacuum vacuum;
    private void FixedUpdate()
    {
        if (vacuum.IsOn() && GetBatteryCharge() > 0)
        {
            ChargeAttatchedBat(-batteryConsumptionRate);
            vacuum.DamageVacuumMesses();
        }

    }

    private void Start()
    {
        vacuum = vacuumObject.GetComponent(typeof(Vacuum)) as Vacuum;
    }


}
