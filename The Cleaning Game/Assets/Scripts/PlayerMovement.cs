using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Transform cameraTransform;
    [SerializeField] float mouseSens = 3f;
    [SerializeField] float movementSpeed = 4f;
    [SerializeField] float mass = 1f;

    Vector2 look;
    Vector3 velocity;
    CharacterController controller;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        updateGravity();
        updateCamera();
        updateMovement();
    }

    void updateCamera()
    {
        look.x += Input.GetAxis("Mouse X") * mouseSens;
        look.y += Input.GetAxis("Mouse Y") * mouseSens;

        look.y = Mathf.Clamp(look.y, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(-look.y, 0, 0);
        transform.localRotation = Quaternion.Euler(0, look.x, 0);
    }

    void updateMovement()
    {
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");

        var input = new Vector3();
        input += transform.forward * y;
        input += transform.right * x;
        input = Vector3.ClampMagnitude(input, 1f);

        controller.Move((input * movementSpeed + velocity) * Time.deltaTime);
    }

    void updateGravity()
    {
        var gravity = Physics.gravity * mass * Time.deltaTime;
        velocity.y = controller.isGrounded ? -1f : velocity.y + gravity.y;
    }
}
