using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHandling : MonoBehaviour
{
    [SerializeField] Transform playerFacing;
    [SerializeField] Collider playerCollider;
    [SerializeField] LayerMask pickupLayer;
    [SerializeField] float pickupRange = 10f;
    [SerializeField] Transform handLocation;
    [SerializeField] SpringJoint handJoint;

    bool isHoldingItem;
    GameObject heldItem = null;

    RaycastHit hit;

    void Start()
    {
        
    }

    void Update()
    {
        highlightPickup();

        if (Input.GetMouseButtonDown(0) && !isHoldingItem)
            attemptPickup();

        else if (Input.GetMouseButtonDown(0) && isHoldingItem)
            dropHeldItem();
    }

    void dropHeldItem()
    {
        Physics.IgnoreCollision(playerCollider, hit.collider, false);
        isHoldingItem = false;
        handJoint.connectedBody = null;
        heldItem = null;
    }

    /*
     * STUB
     * Highlights pickup that player is aiming 
     * at for clarity
     */
    void highlightPickup()
    {
    }

    /*
     * Shoots a ray from the camera position and if a
     * pickup is found in range, pick it up
     */
    void attemptPickup()
    {
        Ray ray = new Ray(playerFacing.position, playerFacing.forward);

        if (Physics.Raycast(ray, out hit, pickupRange, pickupLayer))
            pickupItem();
    }

    /*
     * Player and the held item ignore collision
     * Marks down the player is holding an item.
     * Connects Held Item to the hand joint.
     */
    void pickupItem()
    {
        Physics.IgnoreCollision(playerCollider, hit.collider, true);
        isHoldingItem = true;
        handJoint.connectedBody = hit.rigidbody;

        // Translate the held item to the hand
        /*hit.transform.position = handLocation.position;*/
        hit.rigidbody.MovePosition(handLocation.position);

        // Mark Down the held item
        heldItem = hit.collider.gameObject;
    }
}
