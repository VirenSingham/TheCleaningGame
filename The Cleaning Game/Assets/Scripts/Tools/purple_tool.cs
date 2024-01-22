using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class purple_mess : MonoBehaviour
{
    [SerializeField] Transform detectorTransform;
    [SerializeField] float detectionRange;
    [SerializeField] int MaxPropsToPush;

    [SerializeField] LayerMask messLayer;
    [SerializeField] LayerMask pickupLayer;

    [SerializeField] float hitForce;
    [SerializeField] float hitRadius;


    RaycastHit hit;

    // Update is called once per frame
    void Update()
    {
        // If the tip of the tool is looking at a purple mess then destroy purple
        if (Physics.Raycast(new Ray(detectorTransform.position, detectorTransform.up), out hit, detectionRange, messLayer))
            DestroyPurple();
    }

    void DestroyPurple()
    {
        if (isPurpleMess())
        {
            PushClosePickups();
            Mess mess = hit.collider.gameObject.GetComponent(typeof(Mess)) as Mess;
            mess.KillMess();
        }
    }

    void PushClosePickups()
    {
        Collider[] pickups = new Collider[MaxPropsToPush];
        List<Rigidbody> visitedRbs = new List<Rigidbody>();

        // Find All Colliders close to hit point
        int pickupsToPush = Physics.OverlapSphereNonAlloc(hit.point, hitRadius, pickups, pickupLayer);

        // Loop through pickups
        for (int i = 0; i < pickupsToPush; i++)
        {
            Rigidbody rb = pickups[i].attachedRigidbody;

            // If rigidbody hasnt been passed yet then push it
            if (!visitedRbs.Contains(rb))
            {
                rb.AddExplosionForce(hitForce, hit.point, hitRadius);
                visitedRbs.Add(rb);
            }
        }
    }

    /************** Helper Functions **************/
    bool isPurpleMess()
    {
        return hit.transform.tag.Equals("purple");
    }
}
