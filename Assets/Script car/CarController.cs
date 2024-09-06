using UnityEngine;

public class CarController : MonoBehaviour
{
    public float acceleration = 5f;    // Base acceleration (m/s^2)
    public float braking = 10f;        // Braking force (m/s^2)
    public float maxSpeed = 50f;      // Maximum speed (m/s)
    public float turnSpeed = 30f;     // Steering speed
    public GameObject centerOfMass;
    public float boostMultiplier = 2f; // Multiplier for acceleration when Shift is held

    private Rigidbody rb;
    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentBreakForce;
    private float currentSpeed;
    private bool isInCar = false; // Flag to track if the player is in the car

    // Settings
    [SerializeField] private float motorForce, breakForce, maxSteerAngle;

    // Wheel Colliders
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    // Wheels
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody>();  // Get the Rigidbody component of the car
        currentSpeed = 0f;
        rb.centerOfMass = centerOfMass.transform.localPosition;
    }

    void Update()
    {
        if (isInCar) // Only handle controls if the player is in the car
        {
            HandleAcceleration();
            HandleBraking();
            HandleTurning();
        }
        else
        {
            // Optional: Log the status for debugging
            Debug.Log("Player is not in the car. Car will not move.");
        }
    }

    void HandleAcceleration()
    {
        float currentAcceleration = acceleration;

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) // Check if Shift is pressed
        {
            currentAcceleration *= boostMultiplier; // Apply boost multiplier
        }

        if (Input.GetKey(KeyCode.W))  // Press W to accelerate
        {
            currentSpeed += currentAcceleration * Time.deltaTime;
            if (currentSpeed > maxSpeed)
            {
                currentSpeed = maxSpeed;
            }
        }
        else
        {
            currentSpeed = Mathf.Max(currentSpeed - braking * Time.deltaTime, 0);
        }

        rb.velocity = transform.forward * currentSpeed;  // Set the car's velocity
        Debug.Log($"Current Speed: {currentSpeed}");
    }

    void HandleBraking()
    {
        if (Input.GetKey(KeyCode.S))  // Press S to brake
        {
            currentSpeed = Mathf.Max(currentSpeed - braking * Time.deltaTime, 0);
        }
    }

    void HandleTurning()
    {
        float turn = Input.GetAxis("Horizontal");  // Get turning input from arrow keys or A/D
        transform.Rotate(Vector3.up * turn * turnSpeed * Time.deltaTime);  // Rotate the car
    }

    private void FixedUpdate()
    {
        if (isInCar) // Only handle motor and steering if the player is in the car
        {
            GetInput();
            HandleMotor();
            HandleSteering();
        }
    }

    private void GetInput()
    {
        // Steering Input
        horizontalInput = Input.GetAxis("Horizontal");

        // Acceleration Input
        verticalInput = Input.GetAxis("Vertical");
    }

    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        Debug.Log($"Motor Torque: {verticalInput * motorForce}");
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
        Debug.Log($"Steer Angle: {currentSteerAngle}");
    }
    // Call this method when the player enters or exits the car
    public void SetPlayerInCar(bool inCar)
    {
        isInCar = inCar;
        Debug.Log($"Player in car: {isInCar}");
    }
}