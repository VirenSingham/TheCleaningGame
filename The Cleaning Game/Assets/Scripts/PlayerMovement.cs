using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Transform cameraTransform;
    [SerializeField] float mouseSens = 3f;
    [SerializeField] float movementSpeed = 4f;

    Vector2 look;
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        updateCamera();
    }

    private void FixedUpdate()
    {
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

        rb.MovePosition(transform.position + input * movementSpeed * Time.fixedDeltaTime);
    }
}
