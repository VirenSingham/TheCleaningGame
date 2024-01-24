using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatCharger : BatterySlot
{
    [SerializeField] float chargeRate;

    private void FixedUpdate()
    {
        ChargeAttatchedBat(chargeRate);
    }
}
