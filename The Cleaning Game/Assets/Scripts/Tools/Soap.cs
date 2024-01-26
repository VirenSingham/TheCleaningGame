using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soap : MonoBehaviour
{
    [SerializeField] float breakVelocity;
    [SerializeField] float ImpactRange;
    [SerializeField] float ExplosionRange;
    [SerializeField] LayerMask messLayer;
    [SerializeField] float MaxExplosionDamage;
    [SerializeField] GameObject soapExplosion;

    Rigidbody rb;
    String soapTag = "soap";
    int MaxChecks = 100;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (rb.velocity.magnitude > breakVelocity && CheckImpact())
        {
            DamageSoapables();
            SpawnSoapParticles();
            GameObject.Destroy(gameObject);
        }
    }

    private void SpawnSoapParticles()
    {
        Instantiate(soapExplosion, transform.position, transform.rotation);
    }

    /*
     * Damages all Soapables in the radius of the soap bag
     */
    private void DamageSoapables()
    {
        Collider[] messes = new Collider[MaxChecks];


        // Find All Colliders close to soap
        int refillsFound = Physics.OverlapSphereNonAlloc(transform.position, ExplosionRange, messes, messLayer);

        // Loop through messes found to find soapable ones
        for (int i = 0; i < refillsFound; i++)
        {
            if (messes[i].tag.Equals(soapTag))
            {
                DamageSoapable(messes[i].gameObject);
            }
        }
    }

    /*
     * Deals damage to a patch based off of distance from explosion
     */
    private void DamageSoapable(GameObject soapable)
    {
        float distance = Vector3.Distance(transform.position, soapable.transform.position);
        float damageMod = (ExplosionRange - distance) / ExplosionRange;

        Mess mess = soapable.GetComponent(typeof(Mess)) as Mess;
        mess.Damage(MaxExplosionDamage * damageMod);
    }

    /*
     * Check If Soap is Close to any soapable surfaces
     */
    private bool CheckImpact()
    {
        Collider[] messes = new Collider[MaxChecks];


        // Find All Colliders close to soap
        int messesFound = Physics.OverlapSphereNonAlloc(transform.position, ImpactRange, messes, messLayer);

        // Loop through messes found to find soapable ones
        for (int i = 0; i < messesFound; i++)
        {
            if (messes[i].tag.Equals(soapTag))
                return true;
        }

        return false;
    }
}
