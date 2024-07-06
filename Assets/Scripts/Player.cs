using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5.0f;  // �ƶ��ٶȣ�����ƽ���ƶ�
    public Vector3 gridSize = new Vector3(1, 0, 1);  // �����С��������1x1
    public float fallInterval = 0.5f;  // ÿ���������һ��
    public LayerMask groundLayer;  // �������߼��Ĳ�
    public LayerMask slopeLayer;  // ����б����Ĳ�

    private Vector3 targetPosition;
    private bool isMoving;
    private float fallTimer;
    private bool isGrounded;
    private bool isOnSlope;

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
        // ���ǰ���Ƿ����ϰ����б��
        if (IsObstacleOrSlopeInDirection(direction, out Vector3 slopeDirection))
        {
            direction = slopeDirection;  // ��б�淽�����Ŀ��λ��
        }

        Vector3 target;

        if (isOnSlope)
        {
            RaycastHit slopeHit;

            if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, .5f))
            {
                Vector3 dir = Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
                target = targetPosition + Vector3.Scale(dir, gridSize);
            }
            else
            {
                target = targetPosition + Vector3.Scale(direction, gridSize);
            }
        }
        else
        {
            target = targetPosition + Vector3.Scale(direction, gridSize);
        }

        target = RoundToNearestInteger(target);

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
        isOnSlope = Physics.Raycast(origin, direction, distance, slopeLayer);

        // ���б�津��
        if (isOnSlope)
        {
            RaycastHit hit;
            if (Physics.Raycast(origin, direction, out hit, distance, slopeLayer))
            {
                isOnSlope = Vector3.Angle(hit.normal, Vector3.up) > 0.1f;
            }
        }
    }

    bool IsObstacleInDirection(Vector3 direction, Vector3 target)
    {
        // �������߼��ǰ���Ƿ����ϰ���
        Vector3 origin = transform.position + new Vector3(0, 0.5f, 0); // ������ʼλ��
        float distance = .8f; // ������

        return Physics.Raycast(origin, direction, distance, groundLayer);
    }

    bool IsObstacleOrSlopeInDirection(Vector3 direction, out Vector3 slopeDirection)
    {
        slopeDirection = direction;

        // �������߼��ǰ���Ƿ����ϰ����б��
        Vector3 origin = transform.position + new Vector3(0, 0.5f, 0); // ������ʼλ��
        float distance = gridSize.magnitude; // ������

        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance, slopeLayer))
        {
            Vector3 normal = hit.normal;
            if (normal != Vector3.up)
            {
                slopeDirection = Vector3.ProjectOnPlane(direction, normal).normalized;
                return true;
            }
        }
        return false;
    }

    Vector3 RoundToNearestInteger(Vector3 position)
    {
        return new Vector3(
            Mathf.Round(position.x),
            Mathf.Round(position.y),
            Mathf.Round(position.z)
        );
    }
}
