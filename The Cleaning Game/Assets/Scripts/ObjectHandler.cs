using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHandling : MonoBehaviour
{
    [SerializeField] Transform playerFacing;
    [SerializeField] Collider playerCollider;
    [SerializeField] LayerMask pickupLayer;
    [SerializeField] LayerMask dragLayer;
    [SerializeField] float pickupRange = 10f;
    [SerializeField] Transform handLocation;
    [SerializeField] HingeJoint pickupJoint;
    [SerializeField] SpringJoint dragJoint;
    [SerializeField] float throwSpeed = 1;

    bool isHoldingItem;
    GameObject heldItem = null;

    bool isDraggingItem;
    GameObject draggedItem = null;

    RaycastHit hit;

    void Update()
    {
        pickupHandler();
        dragHandler();
    }

    /*************** Helper Functions ***************/
    Ray generateRayFromCam()
    {
        return new Ray(playerFacing.position, playerFacing.forward);
    }

     /* 
      * Raycasts to see if the object is within range and isnt already dragged or picked up.
      * Must Specify the LayerMask for the pickup request.
      * As a side-effect the hit variable will be updated
      */
    bool isObjectInView(LayerMask targetLayerMasks)
    {
        bool isObjectInView = Physics.Raycast(generateRayFromCam(), out hit, pickupRange, targetLayerMasks);

        if (!isObjectInView)
            return false;

        else if (hit.transform.gameObject == heldItem || hit.transform.gameObject == draggedItem)
            return false;

        else
            return true;
    }

    /*************** Drag ***************/
    /*
     * Handles the behaviours related to dragging
     */
    void dragHandler()
    {
        if (Input.GetMouseButtonDown(1) && !isDraggingItem)
            attemptDrag();

        else if (Input.GetMouseButtonUp(1) && isDraggingItem)
            stopDrag();
    }

    /*
     * Checks to see if the object being clicked can
     * be dragged and then commences drag.
     */
    void attemptDrag()
    {
        if (isObjectInView(dragLayer))
            dragItem();
    }

    /*
     * When this is called Dragging is ceased
     */
    void stopDrag()
    {
        // Reset relevant vars
        dragJoint.connectedBody = null;
        isDraggingItem = false;
        draggedItem = null;
    }

    /*
     * Commences drag by attatching targeted item
     * to spring joint.
     */
    void dragItem()
    {
        isDraggingItem = true;

        // Mark Down the dragged item
        draggedItem = hit.collider.gameObject;

        // Connect dragged item to joint
        dragJoint.connectedBody = hit.rigidbody;
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

        // Reset relevant vars
        pickupJoint.connectedBody = null;
        isHoldingItem = false;
        heldItem = null;

        // Give the object some force to "throw"
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
        if (isObjectInView(pickupLayer))
            pickupItem();
    }

    /*
     * Player and the held item ignore collision
     * Marks down the player is holding an item.
     * Connects Held Item to the hand joint.
     */
    void pickupItem()
    {
        // turn off collisions with player
        Physics.IgnoreCollision(playerCollider, hit.collider, true);

        isHoldingItem = true;

        // Translate the held item to the hand
        hit.rigidbody.MovePosition(handLocation.position);

        // Mark Down the held item
        heldItem = hit.collider.gameObject;

        // Connect held item to joint
        pickupJoint.connectedBody = hit.rigidbody;
    }
}
