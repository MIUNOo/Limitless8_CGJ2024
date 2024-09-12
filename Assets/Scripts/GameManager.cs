using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public PlayerController player;
    public GameObject map;
    public Image countdownSprite;
    public float rotationDuration = 0.5f;
    public float maxRotationAngle = 90f;
    public float countdownDuration = 8f;

    public List<GameObject> balls;
    public List<Sprite> countdownSprites;

    public static GameManager Instance;

    private bool isRotating = false;
    private float currentRotation = 0f;
    private Coroutine countdownCoroutine;
    private float remainingCountdownTime;
    private CharacterController characterController;

    private bool playerInput;

    void Awake()
    {
        //if (Instance == null)
        //{
        //    DontDestroyOnLoad(gameObject);
        //    Instance = this;
        //}
        //else if (Instance != this)
        //{
        //    Destroy(gameObject);
        //}
    }

    void Start()
    {
        playerInput = true;
        countdownSprite.sprite = countdownSprites[8];
        remainingCountdownTime = countdownDuration;
        characterController = player.GetComponent<CharacterController>();

        if (IsMapHorizontal())
        {
            DisableBallPhysics();
        }
    }

    void Update()
    {
        if (!isRotating)
        {
            if (characterController.isGrounded)
            {
                //Debug.LogAssertion("ISGROUNDED");

                if (Input.GetKeyDown(KeyCode.Q) && currentRotation > -maxRotationAngle && playerInput)
                {
                    StartCoroutine(RotateAndStartCountdown(-90));
                    playerInput = false;
                }

                if (Input.GetKeyDown(KeyCode.E) && currentRotation < maxRotationAngle && playerInput)
                {
                    StartCoroutine(RotateAndStartCountdown(90));
                    playerInput = false;
                }
            }
            else
            {
                //Debug.LogAssertion("NO");
            }
        }
    }

    private IEnumerator RotateAndStartCountdown(float angle)
    {
        yield return StartCoroutine(RotateMap(angle));

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
            playerRb.isKinematic = true;
        }

        DisableBallPhysics();

        float elapsed = 0f;
        float startAngle = currentRotation;
        float endAngle = currentRotation + angle;

        while (elapsed < rotationDuration)
        {
            float currentAngle = Mathf.Lerp(startAngle, endAngle, elapsed / rotationDuration);
            map.transform.RotateAround(playerPos, Vector3.right, currentAngle - currentRotation);
            currentRotation = currentAngle;
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        map.transform.RotateAround(playerPos, Vector3.right, endAngle - currentRotation);
        currentRotation = endAngle;

        if (!IsMapHorizontal())
        {
            EnableBallPhysics();
        }

        if (playerRb != null)
        {
            playerRb.isKinematic = false;
        }

        Time.timeScale = 1;
        isRotating = false;
    }

    private IEnumerator Countdown()
    {
        while (remainingCountdownTime > 0)
        {
            countdownSprite.sprite = countdownSprites[(int)remainingCountdownTime - 1];
            yield return new WaitForSecondsRealtime(1f);
            remainingCountdownTime -= 1f;

            if (currentRotation == 0)
            {
                countdownSprite.sprite = countdownSprites[8];
                StartCoroutine(RestoreCountdownTime());
                yield break;
            }
        }

        countdownSprite.sprite = countdownSprites[8];
        StartCoroutine(RotateToOriginalPosition());
    }

    private IEnumerator RestoreCountdownTime()
    {
        while (remainingCountdownTime < countdownDuration)
        {
            remainingCountdownTime += 0.5f;
            yield return new WaitForSecondsRealtime(0.1f);
        }
        remainingCountdownTime = countdownDuration;
    }

    private IEnumerator RotateToOriginalPosition()
    {
        if (currentRotation != 0)
        {
            yield return StartCoroutine(RotateMap(-currentRotation));
            playerInput = true;

            if (countdownCoroutine != null)
            {
                StopCoroutine(countdownCoroutine);
            }

            countdownSprite.sprite = countdownSprites[8];
            remainingCountdownTime = countdownDuration;
        }
    }

    public bool IsMapHorizontal()
    {
        float zAngle = map.transform.eulerAngles.x;
        return Mathf.Abs(zAngle) < 5f || Mathf.Abs(zAngle - 360f) < 5f;
    }

    private void DisableBallPhysics()
    {
        foreach (GameObject ball in balls)
        {
            Rigidbody ballRb = ball.GetComponent<Rigidbody>();
            if (ballRb != null)
            {
                ballRb.isKinematic = true;
            }

            if (ball.TryGetComponent(out Platform platform))
            {
                platform.isStopped = true;
            }
        }
    }

    private void EnableBallPhysics()
    {
        foreach (GameObject ball in balls)
        {
            Rigidbody ballRb = ball.GetComponent<Rigidbody>();
            if (ballRb != null)
            {
                ballRb.isKinematic = false;
            }

            if (ball.TryGetComponent(out Platform platform))
            {
                platform.isStopped = false;
            }
        }
        // UI 调用的左旋转函数
        //
      
    }
  public void RotateLeft() { 
            if (currentRotation > -maxRotationAngle && playerInput && !isRotating) 
        {
            StartCoroutine(RotateAndStartCountdown(-90)); playerInput = false;
        } 
    } 
    // UI 调用的右旋转函数
    public void RotateRight() { 
        if (currentRotation < maxRotationAngle && playerInput && !isRotating) 
        { 
            StartCoroutine(RotateAndStartCountdown(90)); playerInput = false; 
        } 
    }
}
