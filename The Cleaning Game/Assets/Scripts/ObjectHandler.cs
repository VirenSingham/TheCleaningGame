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
    [SerializeField] HingeJoint handJoint;
    [SerializeField] float throwSpeed = 1;

    bool isHoldingItem;
    GameObject heldItem = null;

    RaycastHit hit;

    void Update()
    {
        pickupHandler();
        dragHandler();
    }

    /*************** Drag ***************/
    void dragHandler()
    {

    }

    /*************** Pickups ***************/
    /*
     * Handles the behaviours related to picking
     * up and throwing items.
     */
    void pickupHandler()
    {
        if (Input.GetMouseButtonDown(0) && !isHoldingItem)
            attemptPickup();

        else if (Input.GetMouseButtonDown(0) && isHoldingItem)
            throwHeldItem();
    }

    /*
     * Throwing the held item
     */
    void throwHeldItem()
    {
        // re-enable collisions, disconnect from hand
        Physics.IgnoreCollision(playerCollider, hit.collider, false);
        handJoint.connectedBody = null;

        // Reset relevant bools
        isHoldingItem = false;
        heldItem = null;

        hit.rigidbody.AddForce(getThrowDirection() * throwSpeed);
    }

    /*
     * Made this function in case I want the throw direction
     * to be influenced by the players velocity, which would
     * involve making player movement velocity based.
     */
    Vector3 getThrowDirection()
    {
        return handLocation.forward;
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

        // Translate the held item to the hand
        hit.rigidbody.MovePosition(handLocation.position);

        // Mark Down the held item
        heldItem = hit.collider.gameObject;

        // Connect held item to joint
        handJoint.connectedBody = hit.rigidbody;
    }
}
