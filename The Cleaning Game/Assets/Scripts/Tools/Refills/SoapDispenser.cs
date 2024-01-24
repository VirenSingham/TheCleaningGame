using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoapDispenser : MonoBehaviour, Activatable
{
    [SerializeField] GameObject soapPrefab;
    [SerializeField] Transform soapSpawnLoc;
    [SerializeField] float spawnForce;

    public void Activate()
    {
        SpawnSoap();
    }

    private void SpawnSoap()
    {
        GameObject createdSoap = Instantiate(soapPrefab, soapSpawnLoc.position, soapSpawnLoc.rotation);

        Rigidbody rb = createdSoap.GetComponent<Rigidbody>();
        rb.AddForce(soapSpawnLoc.up * spawnForce);
    }
}
