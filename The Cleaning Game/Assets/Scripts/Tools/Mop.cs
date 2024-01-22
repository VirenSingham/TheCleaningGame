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
    [SerializeField] float detectionRange;
    [SerializeField] float velocityThreshold;

    [SerializeField] LayerMask messLayer;
    [SerializeField] int MaxMessesToCheck;

    [SerializeField] float MopDamage;

    public bool clean = true;

    String mopTag = "mop";

    RaycastHit hit;

    List<GameObject> mopables = new List<GameObject>();

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
        //TODO: do this method
        return false;
    }

    void makeClean()
    {
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
        Collider[] messes = new Collider[MaxMessesToCheck];


        // Find All Colliders close to sponge
        int messesToClean = Physics.OverlapSphereNonAlloc(spongeTrans.position, detectionRange, messes, messLayer);

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

    void makeDirty()
    {
        clean = false;
        spongeMesh.material = dirtyMat;
    }
}
