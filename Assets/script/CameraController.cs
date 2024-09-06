using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody; // The player GameObject to rotate around
    public float xRotation = 0f; // Vertical camera rotation

    private bool isRightMouseDown = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Check if right mouse button is pressed
        {
            isRightMouseDown = true;
        }
        if (Input.GetMouseButtonUp(1)) // Check if right mouse button is released
        {
            isRightMouseDown = false;
        }

        if (isRightMouseDown)
        {
            // Mouse input for rotation
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            // Update the vertical rotation of the camera
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limit vertical rotation

            // Apply the vertical rotation to the camera
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            // Rotate the player around the Y-axis
            playerBody.Rotate(Vector3.up * mouseX);

            // Also rotate the camera around Y-axis for a more dynamic control
            transform.Rotate(Vector3.up * mouseX);
        }
    }
}