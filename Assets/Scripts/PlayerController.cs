using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 6.0f;
    public float gravity = 20.0f;
    public Transform cameraTransform;
    public Vector3 cameraOffset = new Vector3(0, 3, -10);

    public GameObject door;

    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;
    bool interaction;
    bool isGoal;


    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        controller.Move(Vector3.down * Time.deltaTime*5f);
        // If the character is on the ground
        if (controller.isGrounded)
        {
            // Get player input
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            // Convert input to movement direction relative to the camera
            Vector3 move = new Vector3(moveHorizontal, 0.0f, moveVertical);
            move = Camera.main.transform.TransformDirection(move);
            move.y = 0; // Ensure the movement is only horizontal

            if (Input.GetKey(KeyCode.K) && interaction)
            {
                door.transform.Rotate(0, -90, 0);
                interaction = false;
                isGoal = true;
            }


            if (move != Vector3.zero)
            {
                // Align rotation to movement direction
                AlignRotation(move);
            }


            // Set movement direction (character facing direction)
            moveDirection = move * speed;
        }

        // Apply gravity
        moveDirection.y -= gravity * Time.deltaTime;

        // Move character
        controller.Move(moveDirection * Time.deltaTime);

        // Update camera position
        UpdateCamera();
    }

    void UpdateCamera()
    {
        if (cameraTransform != null)
        {
            cameraTransform.position = transform.position + cameraOffset;
            cameraTransform.LookAt(transform.position);
        }
    }

    void AlignRotation(Vector3 direction)
    {
        // Calculate the rotation needed to face the direction
        Quaternion targetRotation = Quaternion.LookRotation(-direction, Vector3.up);

        // Smoothly rotate towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Switch")&&!isGoal)
        {
            interaction = true;
            //Debug.LogAssertion("COLLIDE");

        }
    }

}
