using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mop : MonoBehaviour
{
    [SerializeField] Material cleanMat;
    [SerializeField] Material dirtyMat;
    [SerializeField] MeshRenderer spongeMesh;

    [SerializeField] Transform spongeTrans;
    [SerializeField] Rigidbody spongeRb;
    [SerializeField] float dirtDetectionRange;
    [SerializeField] float waterDetectionRange;
    [SerializeField] float velocityThreshold;

    [SerializeField] LayerMask messLayer;
    [SerializeField] LayerMask refillLayer;

    [SerializeField] float MopDamage;
    [SerializeField] [Range(0,1)] float WaterConsumptionPercent;

    public bool clean = true;
    
    int MaxChecks = 100;
    String mopTag = "mop";
    String bucketTag = "bucketWater";

    List<GameObject> mopables = new List<GameObject>();
    GameObject targetedBucket;

    // Update is called once per frame
    void Update()
    {
        if (clean)
            checkPuddle();
        else
            checkBucket();
    }

    /************** Dirty State ****************/

    /*
     * check for a bucket with water
     */
    private void checkBucket()
    {
        if (isCollidingWater())
            makeClean();
    }

    bool isCollidingWater()
    {
        Collider[] refills = new Collider[MaxChecks];


        // Find All Colliders close to sponge
        int refillsFound = Physics.OverlapSphereNonAlloc(spongeTrans.position, waterDetectionRange, refills, refillLayer);

        // Loop through refills for bucket water
        for (int i = 0; i < refillsFound; i++)
        {
            if (refills[i].tag.Equals(bucketTag))
            {
                targetedBucket = refills[i].transform.parent.gameObject;
                return true;
            }
        }

        return false;
    }

    /*
     * Changes mop state to clean
     * reduces water level on bucket
     */
    void makeClean()
    {
        Bucket bucket = targetedBucket.GetComponent(typeof(Bucket)) as Bucket;
        bucket.setWaterLevel(bucket.getWaterLevel() - WaterConsumptionPercent * bucket.getMaxLevel());

        clean = true;
        spongeMesh.material = cleanMat;
    }


    /************** Clean State ****************/
    /*
     * check if there is anything to clean and if there is
     * then clean all mopables in the list.
     */
    private void checkPuddle()
    {
        if (isCollidingPuddle() && isShaking())
            cleanPuddle();
    }

    /*
     * clean all mopables in the list.
     */
    private void cleanPuddle()
    {
        for (int i = 0; i < mopables.Count; i++)
        {
            Mess mess = mopables[i].GetComponent(typeof(Mess)) as Mess;
            if (mess.Damage(MopDamage))
                makeDirty();
        }
    }

    /*
     * returns whether the mop is currently close enough
     * to a puddle to clean it or not.
     * As a side effect mopable objects are added to mopables
     */
    private bool isCollidingPuddle()
    {
        mopables.Clear();
        Collider[] messes = new Collider[MaxChecks];


        // Find All Colliders close to sponge
        int messesToClean = Physics.OverlapSphereNonAlloc(spongeTrans.position, dirtDetectionRange, messes, messLayer);

        // Loop through messes for moppable
        for (int i = 0; i < messesToClean; i++)
        {
            if (messes[i].tag.Equals(mopTag))
                mopables.Add(messes[i].gameObject);
        }

        return mopables.Count > 0;
    }

    /*
     * returns whether the sponge rigid body is moving or not 
     */
    bool isShaking()
    {
        return spongeRb.velocity.magnitude > velocityThreshold;
    }

    /*
     *  Changes mop state to dirty
     */
    void makeDirty()
    {
        clean = false;
        spongeMesh.material = dirtyMat;
    }
}
