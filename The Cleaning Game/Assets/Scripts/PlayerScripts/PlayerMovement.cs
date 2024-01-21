using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Transform cameraTransform;
    [SerializeField] float mouseSens = 3f;
    [SerializeField] float walkMoveSpeed = 4f;
    [SerializeField] float crouchMoveSpeed = 4f;
    float currentMoveSpeed;
    bool isCrouching;

    Vector2 look;
    Rigidbody rb;

    [SerializeField] LayerMask ButtonLayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentMoveSpeed = walkMoveSpeed;
    }

    void Update()
    {
        updateCamera();

        //Crouch Functionality
        //check for key press
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }

        //Interact Functionality
        //check for key press
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit , 5f, ButtonLayer);
            if (hit.collider.gameObject.tag == "Button")
            {
                hit.collider.gameObject.GetComponentInParent<Elevator>().ButtonPressed();
            }
        }
    }

    private void FixedUpdate()
    {
        updateMovement();
    }

    void updateCamera()
    {
        if (Application.isFocused)
        {
            look.x += Input.GetAxis("Mouse X") * mouseSens;
            look.y += Input.GetAxis("Mouse Y") * mouseSens;

            look.y = Mathf.Clamp(look.y, -90f, 90f);

            cameraTransform.localRotation = Quaternion.Euler(-look.y, 0, 0);
            transform.localRotation = Quaternion.Euler(0, look.x, 0);
        }
    }

    void updateMovement()
    {
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");

        var input = new Vector3();
        input += transform.forward * y;
        input += transform.right * x;
        input = Vector3.ClampMagnitude(input, 1f);

        rb.MovePosition(transform.position + input * currentMoveSpeed * Time.fixedDeltaTime);
    }
    
    void Crouch()
    {
        //if Crouching, Stand
          if (isCrouching == true)
          {
            //change our scale
              transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * 1.5f, transform.localScale.z);
              isCrouching = false;
              currentMoveSpeed = walkMoveSpeed;
          }
        //if Standing, Crouch
          else if (isCrouching == false)
          {
            //change our scale
              transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y / 1.5f, transform.localScale.z);
              isCrouching = true;
              currentMoveSpeed = crouchMoveSpeed;
              rb.AddForce((transform.up * -1) * 2f, ForceMode.Impulse);
          }
    }
}