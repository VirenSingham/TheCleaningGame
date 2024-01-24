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
    [SerializeField] SpringJoint dragJoint;
    [SerializeField] LineRenderer dragRenderer;

    bool isDraggingItem;
    GameObject draggedItem = null;
    GameObject anchorPoint = null;

    RaycastHit hit;

    private void Start()
    {
        // make sure line render is not visible at the start of the game
        dragRenderer.enabled = false;
    }
    void Update()
    {
        dragHandler();
    }

    /*************** Helper Functions ***************/
    Vector3 getHitPosRelativeToBody(RaycastHit passedHit)
    {
        return passedHit.point - passedHit.transform.position;
    }
    
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

        else if (hit.transform.gameObject == draggedItem)
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

        else if (isDraggingItem && anchorPoint == null)
            DragBroken();

        else if (isDraggingItem)
            renderDragLine();
    }

    /*
     * Uses the LineRenderer to make a line 
     * from the mouse to the picked up item.
     */
    void renderDragLine()
    {
        dragRenderer.SetPosition(0, handLocation.position);
        dragRenderer.SetPosition(1, anchorPoint.transform.position);
    }

    /*
     * Checks to see if the object being clicked can
     * be dragged and then commences drag.
     */
    void attemptDrag()
    {
        if (isObjectInView(pickupLayer))
            dragItem();
    }

    /*
     * Commences drag by attatching targeted item
     * to spring joint.
     */
    void dragItem()
    {
        dragRenderer.enabled = true;
        isDraggingItem = true;

        // ignore collisions whilst holding object
        Physics.IgnoreCollision(hit.collider, playerCollider, true);
        
        // Mark Down the dragged item
        draggedItem = hit.collider.gameObject;

        // Connect dragged item to joint at
        // the point it was clicked
        dragJoint.connectedBody = hit.rigidbody;
        dragJoint.connectedAnchor = getHitPosRelativeToBody(hit);

        // Create an object to act as the visualisation of the anchorpoint
        anchorPoint = new GameObject("AnchorPoint");
        anchorPoint.transform.position = hit.point;
        anchorPoint.transform.SetParent(hit.transform);
    }

    /*
     * When this is called Dragging is ceased
     */
    void stopDrag()
    {
        // Reset relevant vars
        Physics.IgnoreCollision(hit.collider, playerCollider, false);
        dragRenderer.enabled = false;
        dragJoint.connectedBody = null;
        isDraggingItem = false;
        draggedItem = null;

        // Destroy anchorPoint object
        GameObject.Destroy(anchorPoint);
        anchorPoint = null;
    }

    /*
     * Should be called if the held object is unexpectedly deleted
     */
    void DragBroken()
    {
        // Reset relevant vars
        dragRenderer.enabled = false;
        dragJoint.connectedBody = null;
        isDraggingItem = false;
        draggedItem = null;
    }
}
