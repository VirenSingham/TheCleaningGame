using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Transform cameraTransform;
    [SerializeField] Transform groundCheckTransform;
    [SerializeField] float mouseSens = 3f;
    [SerializeField] float walkMoveSpeed = 4f;
    [SerializeField] float crouchMoveSpeed = 4f;
    [SerializeField] float jumpForce = 10f;
    float currentMoveSpeed;
    bool isCrouching;

    Vector2 look;
    Vector3 xzVelocity;
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
            Crouch();

        Movement();

        //Interact Functionality
        checkButtonPress();
    }

    private void checkButtonPress()
    {
        RaycastHit hit;
        //check for key press
        if (Input.GetKeyDown(KeyCode.E) && Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, 5f, ButtonLayer))
        {
            if (hit.collider.gameObject.tag == "Button")
            {
                hit.collider.gameObject.GetComponent<Button>().Pressed();
            }
        }
    }

    private void Movement()
    {
        updateXZMovement();
        ApplyMovement();
        ApplyJump();
    }

    private void ApplyJump()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded())
            rb.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(groundCheckTransform.position, -Vector3.up, 0.1f);
    }

    private void ApplyMovement()
    {
        rb.velocity = new Vector3(xzVelocity.x, rb.velocity.y, xzVelocity.z);
        /*if (xzVelocity != Vector3.zero)
        {
            rb.velocity = new Vector3(xzVelocity.x, rb.velocity.y, xzVelocity.z);

        }
        else
        {
            rb.velocity = rbWithNoXZVel();
        }*/
    }

    /*
     *  Returns the rigidbody with the x and z component = 0;
     */
    private Vector3 rbWithNoXZVel()
    {
        return new Vector3(0, rb.velocity.y, 0);
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

    void updateXZMovement()
    {
        var x = Input.GetAxisRaw("Horizontal");
        var z = Input.GetAxisRaw("Vertical");

        var input = new Vector3();
        input += transform.forward * z;
        input += transform.right * x;
        input = input.normalized;

        /*rb.MovePosition(transform.position + input * currentMoveSpeed * Time.fixedDeltaTime);*/
        xzVelocity = input * currentMoveSpeed;
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