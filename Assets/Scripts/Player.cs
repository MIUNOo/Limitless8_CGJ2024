using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5.0f;  // �ƶ��ٶȣ�����ƽ���ƶ�
    public Vector3 gridSize = new Vector3(1, 0, 1);  // �����С��������1x1
    public float fallInterval = .5f;  // ÿ���������һ��
    public LayerMask groundLayer;  // �������߼��Ĳ�

    private Vector3 targetPosition;
    private bool isMoving;
    private float fallTimer;
    private bool isGrounded;

    void Start()
    {
        // ��ʼ��Ŀ��λ��Ϊ��ɫ��ǰλ��
        targetPosition = transform.position;
        fallTimer = fallInterval;
        isGrounded = false;
    }

    void Update()
    {
        // ����������ʱ��
        fallTimer -= Time.deltaTime;

        // �������룬ֻ�е���ɫ�����ƶ�ʱ�Ž���������
        if (!isMoving)
        {
            HandleInput();
        }

        // ���д��׼��
        CheckGrounded();

        // ���û��������ʱ�䵽����δ���ף��Զ������ƶ�
        if (!isMoving && !isGrounded && fallTimer <= 0)
        {
            TryMove(Vector3.down);
            fallTimer = fallInterval; // ���ü�ʱ��
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
            TryMove(direction);
        }
    }

    void TryMove(Vector3 direction)
    {
        // ���ǰ���Ƿ����ϰ���
        Vector3 target = targetPosition + Vector3.Scale(direction, gridSize);
        if (!IsObstacleInDirection(direction, target))
        {
            targetPosition = target;
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

    void CheckGrounded()
    {
        // �������߼���Ƿ񴥵�
        Vector3 origin = transform.position + new Vector3(0, -0.5f, 0); // ������ʼλ�ã����Ը������ģ�͵���
        Vector3 direction = Vector3.down;
        float distance = 1.0f; // ���߼�����

        isGrounded = Physics.Raycast(origin, direction, distance, groundLayer);
    }

    bool IsObstacleInDirection(Vector3 direction, Vector3 target)
    {
        // �������߼��ǰ���Ƿ����ϰ���
        Vector3 origin = transform.position + new Vector3(0, 0.5f, 0); // ������ʼλ��
        float distance = .8f; // ������

        // ʹ�ú�����������������������
        if (Physics.Raycast(origin, direction, distance, groundLayer))
        {
            return true;
        }
        return false;
    }
}