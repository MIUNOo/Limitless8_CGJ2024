using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5.0f;  // �ƶ��ٶ�
    public Vector3 gridSize = new Vector3(1, 1, 1);  // �����С
    public LayerMask groundLayer;  // ����㣬���ڼ���µ�
    public LayerMask obstacleLayer;  // �ϰ���㣬���ڼ��ǽ
    public float slopeForce = 1.5f;  // ���µ����ƶ�������
    public float slopeRayLength = 1.5f;  // ����µ������߳���
    public float obstacleRayLength = 1.0f;  // ����ϰ�������߳���

    private Vector3 targetPosition;
    private bool isMoving;
    private CharacterController characterController;

    void Start()
    {
        // ��ʼ��Ŀ��λ��Ϊ��ɫ��ǰλ��
        targetPosition = RoundToGrid(transform.position);
        characterController = GetComponent<CharacterController>();
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
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            direction = Vector3.up;
        }
        else if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.C))
        {
            direction = Vector3.down;
        }

        if (direction != Vector3.zero)
        {
            // ���ǰ���Ƿ����ϰ�����µ�
            if (!IsObstacleInDirection(direction))
            {
                // ����Ŀ��λ��
                targetPosition += Vector3.Scale(direction, gridSize);
                isMoving = true;
            }
        }
    }

    void MoveToTarget()
    {
        if (isMoving)
        {
            // ��ȡƽ���ƶ��ķ���
            Vector3 moveDirection = (targetPosition - transform.position).normalized * moveSpeed;

            // ����Ƿ����µ���
            if (OnSlope())
            {
                moveDirection += Vector3.down * slopeForce;
            }

            // �ƶ���ɫ
            characterController.Move(moveDirection * Time.deltaTime);

            // ������Ŀ��λ��ʱ��ֹͣ�ƶ�
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                transform.position = targetPosition;  // ǿ������λ���Է�ֹƯ��
                isMoving = false;
            }
        }
    }

    bool OnSlope()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, slopeRayLength, groundLayer))
        {
            if (hit.normal != Vector3.up)
            {
                return true;
            }
        }
        return false;
    }

    bool IsObstacleInDirection(Vector3 direction)
    {
        RaycastHit hit;
        Vector3 origin = transform.position + characterController.center;

        // ��Ŀ�귽����Ͷ������
        if (Physics.Raycast(origin, direction, out hit, obstacleRayLength, obstacleLayer))
        {
            return true;  // ��⵽�ϰ���
        }
        return false;  // û�м�⵽�ϰ���
    }

    Vector3 RoundToGrid(Vector3 position)
    {
        return new Vector3(
            Mathf.Round(position.x / gridSize.x) * gridSize.x,
            Mathf.Round(position.y / gridSize.y) * gridSize.y,
            Mathf.Round(position.z / gridSize.z) * gridSize.z
        );
    }

    //public float moveSpeed = 5.0f;  // �ƶ��ٶ�
    //public Vector3 gridSize = new Vector3(1, 1, 1);  // �����С
    //public LayerMask groundLayer;  // ����㣬���ڼ���µ�
    //public float slopeForce = 1.5f; // ���µ����ƶ�������
    //public float slopeRayLength = 1.5f;  // ���߳��ȣ����ڼ���µ�

    //private Vector3 targetPosition;
    //private bool isMoving;
    //private CharacterController characterController;

    //void Start()
    //{
    //    // ��ʼ��Ŀ��λ��Ϊ��ɫ��ǰλ��
    //    targetPosition = RoundToGrid(transform.position);
    //    characterController = GetComponent<CharacterController>();
    //}

    //void Update()
    //{
    //    // �������룬ֻ�е���ɫ�����ƶ�ʱ�Ž���������
    //    if (!isMoving)
    //    {
    //        HandleInput();
    //    }

    //    // ƽ���ƶ���Ŀ��λ��
    //    MoveToTarget();
    //}

    //void HandleInput()
    //{
    //    Vector3 direction = Vector3.zero;

    //    if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
    //    {
    //        direction = Vector3.forward;
    //    }
    //    else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
    //    {
    //        direction = Vector3.back;
    //    }
    //    else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
    //    {
    //        direction = Vector3.left;
    //    }
    //    else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
    //    {
    //        direction = Vector3.right;
    //    }
    //    else if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        direction = Vector3.up;
    //    }
    //    else if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.C))
    //    {
    //        direction = Vector3.down;
    //    }

    //    if (direction != Vector3.zero)
    //    {
    //        // ����Ŀ��λ��
    //        targetPosition += Vector3.Scale(direction, gridSize);
    //        isMoving = true;
    //    }
    //}

    //void MoveToTarget()
    //{
    //    if (isMoving)
    //    {
    //        // ��ȡƽ���ƶ��ķ���
    //        Vector3 moveDirection = (targetPosition - transform.position).normalized * moveSpeed;

    //        // ����Ƿ����µ���
    //        if (OnSlope())
    //        {
    //            moveDirection += Vector3.down * slopeForce;
    //        }

    //        // �ƶ���ɫ
    //        characterController.Move(moveDirection * Time.deltaTime);

    //        // ������Ŀ��λ��ʱ��ֹͣ�ƶ�
    //        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
    //        {
    //            transform.position = targetPosition;  // ǿ������λ���Է�ֹƯ��
    //            isMoving = false;
    //        }
    //    }
    //}

    //bool OnSlope()
    //{
    //    RaycastHit hit;
    //    if (Physics.Raycast(transform.position, Vector3.down, out hit, slopeRayLength, groundLayer))
    //    {
    //        if (hit.normal != Vector3.up)
    //        {
    //            return true;
    //        }
    //    }
    //    return false;
    //}

    //Vector3 RoundToGrid(Vector3 position)
    //{
    //    return new Vector3(
    //        Mathf.Round(position.x / gridSize.x) * gridSize.x,
    //        //Mathf.Round(position.y / gridSize.y) * gridSize.y,
    //        position.y,

    //        Mathf.Round(position.z / gridSize.z) * gridSize.z
    //    );
    //}



    ///// <summary>
    ///// ���õ���ͨ�ƶ� ���ܴ������� ����character 
    ///// </summary>
    //public float moveSpeed = 5.0f;  // �ƶ��ٶȣ�����ƽ���ƶ�
    //public Vector3 gridSize = new Vector3(1, 0, 1);  // �����С��������1x1

    //private Vector3 targetPosition;
    //private bool isMoving;

    //void Start()
    //{
    //    // ��ʼ��Ŀ��λ��Ϊ��ɫ��ǰλ��
    //    targetPosition = transform.position;
    //}

    //void Update()
    //{
    //    // �������룬ֻ�е���ɫ�����ƶ�ʱ�Ž���������
    //    if (!isMoving)
    //    {
    //        HandleInput();
    //    }

    //    // ƽ���ƶ���Ŀ��λ��
    //    MoveToTarget();
    //}

    //void HandleInput()
    //{
    //    Vector3 direction = Vector3.zero;

    //    if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
    //    {
    //        direction = Vector3.forward;
    //    }
    //    else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
    //    {
    //        direction = Vector3.back;
    //    }
    //    else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
    //    {
    //        direction = Vector3.left;
    //    }
    //    else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
    //    {
    //        direction = Vector3.right;
    //    }

    //    if (direction != Vector3.zero)
    //    {
    //        // ����Ŀ��λ��
    //        targetPosition += Vector3.Scale(direction, gridSize);
    //        isMoving = true;
    //    }
    //}

    //void MoveToTarget()
    //{
    //    // ���Ŀ��λ�ú͵�ǰλ�ò�ͬ���ƶ���ɫ
    //    if (isMoving)
    //    {
    //        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

    //        // ������Ŀ��λ��ʱ��ֹͣ�ƶ�
    //        if (transform.position == targetPosition)
    //        {
    //            isMoving = false;
    //        }
    //    }
    //}




}
