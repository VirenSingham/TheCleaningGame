using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatCharger : BatterySlot
{
    [SerializeField] ParticleSystem chargingParticles;
    [SerializeField] AudioSource ChargingNoise;
    [SerializeField] float chargeRate;

    private void FixedUpdate()
    {
        ChargeAttatchedBat(chargeRate);

        if (isHoldingBattery() && GetBatteryCharge() < 100)
        {
            if (!ChargingNoise.isPlaying)
                ChargingNoise.Play();

            if (!chargingParticles.isPlaying)
                chargingParticles.Play();
        }
        else
        {
            ChargingNoise.Pause();

            chargingParticles.Pause();
            chargingParticles.Clear();
        }
    }
}
