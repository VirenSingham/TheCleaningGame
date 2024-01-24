using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vacuum : MonoBehaviour, Activatable
{
    [SerializeField] float messDetectionRange;
    [SerializeField] LayerMask messLayer;
    [SerializeField] float VacDamage;

    String vacuumTag = "vacuum";
    bool isOn = false;
    int maxChecks = 100;

    public void Activate()
    {
        isOn = !isOn;
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
}
