using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatCharger : BatterySlot
{
    [SerializeField] float chargeRate;
    [SerializeField] ParticleSystem chargingParticles;

    private void FixedUpdate()
    {
        ChargeAttatchedBat(chargeRate);

        if (isHoldingBattery() && GetBatteryCharge() < 100)
        {
            if (!chargingParticles.isPlaying)
                chargingParticles.Play();
        }
        else
        {
            chargingParticles.Pause();
            chargingParticles.Clear();
        }
    }
}
