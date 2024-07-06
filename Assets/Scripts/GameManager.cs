using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public PlayerController player;
    public Text limitedUI;
    public GameObject map;
    public Text countdownText; // Text to display countdown
    public float rotationDuration = 0.5f; // Duration of the rotation
    public float maxRotationAngle = 90f;  // Maximum rotation angle from the original orientation
    public float countdownDuration = 8f;  // Duration of the countdown

    public List<GameObject> balls; // List of balls to disable/enable physics

    private int limitedTurn;
    private bool isRotating = false;
    private float currentRotation = 0f; // Track the current rotation angle
    private Coroutine countdownCoroutine; // Reference to the running countdown coroutine
    private float remainingCountdownTime; // Time remaining in the countdown
    private CharacterController characterController;

    bool playerInput;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = true;
        limitedTurn = int.Parse(limitedUI.text);
        countdownText.gameObject.SetActive(false); // Initially hide the countdown text
        remainingCountdownTime = countdownDuration; // Initialize countdown time
        characterController = player.GetComponent<CharacterController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRotating) // Check if not currently rotating
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
            {
                limitedTurn--;
                limitedUI.text = limitedTurn.ToString();
            }

            if (Input.GetKeyDown(KeyCode.Q) && currentRotation > -maxRotationAngle && playerInput && characterController.isGrounded)
            {
                StartCoroutine(RotateAndStartCountdown(-90)); // Rotate left and start countdown
                playerInput = false;
            }

            if (Input.GetKeyDown(KeyCode.E) && currentRotation < maxRotationAngle && playerInput && characterController.isGrounded)
            {
                StartCoroutine(RotateAndStartCountdown(90)); // Rotate right and start countdown
                playerInput = false;
            }

            // If the player presses the 'R' key, rotate back to the original position
            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(RotateToOriginalPosition());
            }
        }
    }

    private IEnumerator RotateAndStartCountdown(float angle)
    {
        yield return StartCoroutine(RotateMap(angle)); // Rotate to new angle

        // Start or restart countdown
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
        }
        countdownCoroutine = StartCoroutine(Countdown());
    }

    private IEnumerator RotateMap(float angle)
    {
        isRotating = true;
        Time.timeScale = 0;

        Vector3 playerPos = player.transform.position;
        Rigidbody playerRb = player.GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            playerRb.isKinematic = true; // Disable physics on the player
        }

        // Disable physics on balls
        foreach (GameObject ball in balls)
        {
            Rigidbody ballRb = ball.GetComponent<Rigidbody>();
            if (ballRb != null)
            {
                ballRb.isKinematic = true;
            }
        }

        float elapsed = 0f;
        float startAngle = currentRotation;
        float endAngle = currentRotation + angle;

        while (elapsed < rotationDuration)
        {
            float currentAngle = Mathf.Lerp(startAngle, endAngle, elapsed / rotationDuration);
            map.transform.RotateAround(playerPos, Vector3.forward, currentAngle - currentRotation);
            currentRotation = currentAngle;
            elapsed += Time.unscaledDeltaTime; // Use unscaled time because Time.timeScale is 0
            yield return null;
        }

        map.transform.RotateAround(playerPos, Vector3.forward, endAngle - currentRotation); // Ensure final rotation is exact
        currentRotation = endAngle;


        if (currentRotation != 0)
        {
            // Re-enable physics on balls
            foreach (GameObject ball in balls)
            {
                Rigidbody ballRb = ball.GetComponent<Rigidbody>();
                if (ballRb != null)
                {
                    ballRb.isKinematic = false;
                }
            }
        }



        if (playerRb != null)
        {
            playerRb.isKinematic = false; // Re-enable physics on the player
        }

        Time.timeScale = 1;
        isRotating = false;
    }



    private IEnumerator Countdown()
    {
        countdownText.gameObject.SetActive(true);

        while (remainingCountdownTime > 0)
        {
            countdownText.text = remainingCountdownTime.ToString("F1"); // Display countdown in seconds
            yield return new WaitForSecondsRealtime(1f);
            remainingCountdownTime -= 1f;

            // If the player has rotated back to the original position, stop the countdown
            if (currentRotation == 0)
            {
                countdownText.gameObject.SetActive(false);
                StartCoroutine(RestoreCountdownTime());
                yield break; // Exit the coroutine
            }
        }

        countdownText.gameObject.SetActive(false); // Hide countdown text

        // Rotate back to original position after countdown ends
        StartCoroutine(RotateToOriginalPosition());
    }

    private IEnumerator RestoreCountdownTime()
    {
        while (remainingCountdownTime < countdownDuration)
        {
            remainingCountdownTime += 0.5f; // Adjust this value for smoother or faster restoration
            yield return new WaitForSecondsRealtime(0.1f); // Adjust this value to control how often the countdown time updates
        }
        remainingCountdownTime = countdownDuration; // Ensure it doesn't exceed the max value
    }

    private IEnumerator RotateToOriginalPosition()
    {
        if (currentRotation != 0)
        {
            yield return StartCoroutine(RotateMap(-currentRotation)); // Rotate back to 0 angle

            playerInput = true;

            // If there was a countdown running, stop it
            if (countdownCoroutine != null)
            {
                StopCoroutine(countdownCoroutine);
            }

            countdownText.gameObject.SetActive(false); // Hide countdown text
            remainingCountdownTime = countdownDuration; // Reset countdown time
        }
    }
}
