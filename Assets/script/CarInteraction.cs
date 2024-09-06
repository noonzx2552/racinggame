using UnityEngine;
using UnityEngine.UI; // Make sure you have the UI namespace for interaction

public class CarInteraction : MonoBehaviour
{
    public Transform car; // Drag the car transform here in the inspector
    public Transform player; // Drag the player transform here in the inspector
    public Transform seat; // Drag the seat transform here in the inspector
    public Camera playerCamera; // Drag the player camera here in the inspector
    public Camera carCamera; // Drag the car camera here in the inspector
    public float interactionDistance = 3.0f; // Distance within which the player can interact with the car
    public Button interactionButton; // Drag the UI Button here to show the interaction prompt

    private bool isInCar = false;
    private bool isNearCar = false;

    void Start()
    {
        // Ensure the car camera is inactive at the start
        carCamera.gameObject.SetActive(false);

        // Ensure the interaction button is hidden at the start
        if (interactionButton != null)
        {
            interactionButton.gameObject.SetActive(false);
            interactionButton.onClick.AddListener(OnInteractionButtonClicked);
        }
    }

    void Update()
    {
        // Check distance between player and car
        float distanceToCar = Vector3.Distance(player.position, car.position);

        if (distanceToCar <= interactionDistance)
        {
            if (!isNearCar)
            {
                // Player has entered the interaction range
                isNearCar = true;
                ShowInteractionButton();
            }

            // Check for input to enter/exit car
            if (Input.GetKeyDown(KeyCode.E))
            {
                OnInteractionButtonClicked();
            }
        }
        else
        {
            if (isNearCar)
            {
                // Player has exited the interaction range
                isNearCar = false;
                HideInteractionButton();
            }
        }
    }

    void EnterCar()
    {
        if (!isInCar) // Check if the player is not already in the car
        {
            // Move player to the car's seat position
            player.position = seat.position;
            player.gameObject.SetActive(false); // Hide the player

            // Switch cameras
            playerCamera.gameObject.SetActive(false);
            carCamera.gameObject.SetActive(true);

            // Set car's player-in-car status
            car.GetComponent<CarController>().SetPlayerInCar(true);

            isInCar = true;
        }
    }

    void ExitCar()
    {
        if (isInCar) // Check if the player is in the car
        {
            // Move player to a position next to the car
            player.position = car.position + car.forward * 2; // Adjust the position as needed
            player.gameObject.SetActive(true); // Show the player

            // Switch cameras
            playerCamera.gameObject.SetActive(true);
            carCamera.gameObject.SetActive(false);

            // Set car's player-in-car status
            car.GetComponent<CarController>().SetPlayerInCar(false);

            isInCar = false;
        }
    }

    void OnInteractionButtonClicked()
    {
        if (isInCar)
        {
            ExitCar();
        }
        else
        {
            EnterCar();
        }
    }

    void ShowInteractionButton()
    {
        if (interactionButton != null)
        {
            interactionButton.gameObject.SetActive(true);
        }
    }

    void HideInteractionButton()
    {
        if (interactionButton != null)
        {
            interactionButton.gameObject.SetActive(false);
        }
    }
}
