using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    // Player variables
    public float walkSpeed = 20f;
    public float runSpeed = 40f;
    public float jumpPower = 40f;
    public float gravity = 10f;
    public float rotationSpeed = 10f; // Speed at which player rotates
    public float lookSpeed = 2f; // Speed at which the player looks around

    public Transform playerCamera; // Reference to the camera

    private Vector3 moveDirection = Vector3.zero;
    private CharacterController characterController;
    private bool canMove = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        if (!canMove) return;

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical");
        float curSpeedY = (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal");

        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // Apply gravity
        if (characterController.isGrounded)
        {
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpPower;
            }
            else
            {
                moveDirection.y = -0.5f; // Small value to keep grounded
            }
        }
        else
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void HandleRotation()
    {
        // Rotate player body left/right based on mouse X input
        float yaw = Input.GetAxis("Mouse X") * rotationSpeed;
        transform.Rotate(0, yaw, 0);

        // Rotate camera up/down based on mouse Y input
        if (playerCamera != null)
        {
            float pitch = -Input.GetAxis("Mouse Y") * lookSpeed;
            playerCamera.Rotate(pitch, 0, 0);
        }
    }
}
