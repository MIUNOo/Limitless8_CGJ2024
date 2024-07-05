using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float moveSpeed = 5.0f;  // �ƶ��ٶȣ�����ƽ���ƶ�
    public Vector3 gridSize = new Vector3(1, 0, 1);  // �����С��������1x1

    private Vector3 targetPosition;
    private bool isMoving;

    void Start()
    {
        // ��ʼ��Ŀ��λ��Ϊ��ɫ��ǰλ��
        targetPosition = transform.position;
    }

    void Update()
    {
        // �������룬ֻ�е���ɫ�����ƶ�ʱ�Ž���������
        if (!isMoving)
        {
            HandleInput();
        }

        // ƽ���ƶ���Ŀ��λ��
        MoveToTarget();
    }

    void HandleInput()
    {
        Vector3 direction = Vector3.zero;

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            direction = Vector3.forward;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            direction = Vector3.back;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            direction = Vector3.left;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            direction = Vector3.right;
        }

        if (direction != Vector3.zero)
        {
            // ����Ŀ��λ��
            targetPosition += Vector3.Scale(direction, gridSize);
            isMoving = true;
        }
    }

    void MoveToTarget()
    {
        // ���Ŀ��λ�ú͵�ǰλ�ò�ͬ���ƶ���ɫ
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // ������Ŀ��λ��ʱ��ֹͣ�ƶ�
            if (transform.position == targetPosition)
            {
                isMoving = false;
            }
        }
    }



    //public float speed = 6.0f;
    //public float jumpSpeed = 8.0f;
    //public float gravity = 20.0f;
    //public Transform cameraTransform;
    //public Vector3 cameraOffset = new Vector3(0, 3, -10);

    //private Vector3 moveDirection = Vector3.zero;
    //private CharacterController controller;

    //void Start()
    //{
    //    controller = GetComponent<CharacterController>();
    //}

    //void Update()
    //{
    //    // �����ɫ�ڵ�����
    //    if (controller.isGrounded)
    //    {
    //        // ��ȡ�������
    //        float moveHorizontal = Input.GetAxis("Horizontal");
    //        float moveVertical = Input.GetAxis("Vertical");

    //        // ������ת��Ϊ��ɫ����
    //        Vector3 move = new Vector3(moveHorizontal, 0.0f, moveVertical);

    //        // �����ƶ����򣨽�ɫ����
    //        moveDirection = transform.TransformDirection(move) * speed;

    //        // ��Ծ
    //        if (Input.GetButton("Jump"))
    //        {
    //            moveDirection.y = jumpSpeed;
    //        }
    //    }

    //    // Ӧ������
    //    moveDirection.y -= gravity * Time.deltaTime;

    //    // �ƶ���ɫ
    //    controller.Move(moveDirection * Time.deltaTime);

    //    // ���������λ��
    //    UpdateCamera();
    //}

    //void UpdateCamera()
    //{
    //    if (cameraTransform != null)
    //    {
    //        cameraTransform.position = transform.position + cameraOffset;
    //        cameraTransform.LookAt(transform.position);
    //    }
    //}




    //public float moveStep = 1.0f; // �ƶ�����
    //public float rotationStep = 10.0f; // ��ת�������Ƕȣ�

    //private void Update()
    //{
    //    // �ƶ�����
    //    if (Input.GetKeyDown(KeyCode.W))
    //    {
    //        // ��ǰ�ƶ�
    //        this.transform.position += this.transform.forward * moveStep;
    //    }
    //    if (Input.GetKeyDown(KeyCode.S))
    //    {
    //        // ����ƶ�
    //        this.transform.position -= this.transform.forward * moveStep;
    //    }
    //    if (Input.GetKeyDown(KeyCode.A))
    //    {
    //        // �����ƶ�
    //        this.transform.position -= this.transform.right * moveStep;
    //    }
    //    if (Input.GetKeyDown(KeyCode.D))
    //    {
    //        // �����ƶ�
    //        this.transform.position += this.transform.right * moveStep;
    //    }

    //    // ��ת����
    //    if (Input.GetKeyDown(KeyCode.Q))
    //    {
    //        // ������ת
    //        this.transform.Rotate(Vector3.up, -rotationStep);
    //    }
    //    if (Input.GetKeyDown(KeyCode.E))
    //    {
    //        // ������ת
    //        this.transform.Rotate(Vector3.up, rotationStep);
    //    }
    //}

    //public float moveStep = 1.0f; // �ƶ�����
    //public float rotationStep = 10.0f; // ��ת�������Ƕȣ�
    //public float gravity = 9.81f; // ����
    //public LayerMask groundLayer; // ����㣬���ڼ����淨��

    //private CharacterController _controller;
    //private Vector3 moveDirection = Vector3.zero;

    //private void Start()
    //{
    //    _controller = GetComponent<CharacterController>();
    //}

    //private void Update()
    //{
    //    HandleMovement();
    //    HandleRotation();
    //}

    //private void HandleMovement()
    //{
    //    if (_controller.isGrounded)
    //    {
    //        // ��׽���벢�����ƶ�
    //        moveDirection = Vector3.zero;

    //        if (Input.GetKeyDown(KeyCode.W))
    //        {
    //            moveDirection += this.transform.forward * moveStep;
    //        }
    //        if (Input.GetKeyDown(KeyCode.S))
    //        {
    //            moveDirection -= this.transform.forward * moveStep;
    //        }
    //        if (Input.GetKeyDown(KeyCode.A))
    //        {
    //            moveDirection -= this.transform.right * moveStep;
    //        }
    //        if (Input.GetKeyDown(KeyCode.D))
    //        {
    //            moveDirection += this.transform.right * moveStep;
    //        }

    //        // ���ֽ�ɫֱ��
    //        AlignToGround();
    //    }

    //    // �������
    //    moveDirection.y -= gravity * Time.deltaTime;

    //    // �����ƶ�
    //    _controller.Move(moveDirection * Time.deltaTime);
    //}

    //private void HandleRotation()
    //{
    //    // ��ת����
    //    if (Input.GetKeyDown(KeyCode.Q))
    //    {
    //        this.transform.Rotate(Vector3.up, -rotationStep);
    //    }
    //    if (Input.GetKeyDown(KeyCode.E))
    //    {
    //        this.transform.Rotate(Vector3.up, rotationStep);
    //    }
    //}

    //private void AlignToGround()
    //{
    //    RaycastHit hit;
    //    // �ӽ�ɫ��������Ͷ�����
    //    if (Physics.Raycast(transform.position, Vector3.down, out hit, _controller.height / 2 + 0.5f, groundLayer))
    //    {
    //        // ���ݵ��淨�ߵ�����ɫ����ת
    //        Vector3 forward = Vector3.Cross(transform.right, hit.normal);
    //        this.transform.rotation = Quaternion.LookRotation(forward, hit.normal);
    //    }
    //}


}
