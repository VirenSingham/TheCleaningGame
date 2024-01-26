using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vacuum : MonoBehaviour, Activatable
{
    [SerializeField] Material buttonMat;
    [SerializeField] GameObject SuckParticles;
    [SerializeField] LayerMask messLayer;
    [SerializeField] AudioSource VacuumNoise;
    [SerializeField] GameObject BatterySlotObject;

    [SerializeField] float messDetectionRange;
    [SerializeField] float VacDamage;
    [SerializeField] Color OnColor;
    [SerializeField] Color OffColor;

    String vacuumTag = "vacuum";
    VacuumSlot batterySlot;
    bool isOn = false;
    int maxChecks = 100;

    private void Update()
    {
        if (!IsOnWithPower())
            HandleFX();
    }

    public void Activate()
    {
        isOn = !isOn;

        HandleFX();
        SetOnSwitchColour();
    }

    private void HandleFX()
    {
        HandleVacuumAudio();
        HandleVacuumParticles();
    }

    private void HandleVacuumParticles()
    {
        SuckParticles.SetActive(IsOnWithPower());
    }

    private void HandleVacuumAudio()
    {
        if (IsOnWithPower() && !VacuumNoise.isPlaying)
            VacuumNoise.Play();
        else
            VacuumNoise.Pause();
    }

    public void DamageVacuumMesses()
    {
        Collider[] vacuumables = new Collider[maxChecks];

        // Find All Colliders close to vacuum that is a mess
        int vacuumablesFound = Physics.OverlapSphereNonAlloc(transform.position, messDetectionRange, vacuumables, messLayer);

        // Loop through messes, if vacuumable is there then damage it
        for (int i = 0; i < vacuumablesFound; i++)
        {
            if (vacuumables[i].tag.Equals(vacuumTag))
            {
                Mess mess = vacuumables[i].GetComponent(typeof(Mess)) as Mess;
                mess.Damage(VacDamage);
            }
        }
    }

    public bool IsOn()
    {
        return isOn;
    }

    private bool IsOnWithPower()
    {
        return isOn && batterySlot.GetBatteryCharge() > 0;
    }

    private void SetOnSwitchColour()
    {
        if (isOn)
            buttonMat.color = OffColor;
        else
            buttonMat.color = OnColor;
    }

    private void Awake()
    {
        SetOnSwitchColour();

        batterySlot = BatterySlotObject.GetComponent<VacuumSlot>();
    }
}
