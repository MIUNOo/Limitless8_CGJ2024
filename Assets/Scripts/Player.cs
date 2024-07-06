using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5.0f;  // 移动速度，用于平滑移动
    public Vector3 gridSize = new Vector3(1, 0, 1);  // 网格大小，假设是1x1
    public float fallInterval = 0.5f;  // 每隔多久下落一格
    public LayerMask groundLayer;  // 用于射线检测的层
    public LayerMask slopeLayer;  // 用于斜面检测的层

    private Vector3 targetPosition;
    private bool isMoving;
    private float fallTimer;
    private bool isGrounded;
    private bool isOnSlope;

    void Start()
    {
        // 初始化目标位置为角色当前位置
        targetPosition = transform.position;
        fallTimer = fallInterval;
        isGrounded = false;
    }

    void Update()
    {
        // 更新重力计时器
        fallTimer -= Time.deltaTime;

        // 处理输入，只有当角色不在移动时才接受新输入
        if (!isMoving)
        {
            HandleInput();
        }

        // 进行触底检测
        CheckGrounded();

        // 如果没有输入且时间到了且未触底，自动向下移动
        if (!isMoving && !isGrounded && fallTimer <= 0)
        {
            TryMove(Vector3.down);
            fallTimer = fallInterval; // 重置计时器
        }

        // 平滑移动到目标位置
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
        // 检测前方是否有障碍物或斜面
        if (IsObstacleOrSlopeInDirection(direction, out Vector3 slopeDirection))
        {
            direction = slopeDirection;  // 按斜面方向调整目标位置
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
        // 如果目标位置和当前位置不同，移动角色
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // 当到达目标位置时，停止移动
            if (transform.position == targetPosition)
            {
                isMoving = false;
            }
        }
    }

    void CheckGrounded()
    {
        // 发射射线检测是否触底
        Vector3 origin = transform.position + new Vector3(0, -0.5f, 0); // 射线起始位置，可以根据你的模型调整
        Vector3 direction = Vector3.down;
        float distance = 1.0f; // 射线检测距离

        isGrounded = Physics.Raycast(origin, direction, distance, groundLayer);
        isOnSlope = Physics.Raycast(origin, direction, distance, slopeLayer);

        // 检测斜面触底
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
        // 发射射线检测前方是否有障碍物
        Vector3 origin = transform.position + new Vector3(0, 0.5f, 0); // 射线起始位置
        float distance = .8f; // 检测距离

        return Physics.Raycast(origin, direction, distance, groundLayer);
    }

    bool IsObstacleOrSlopeInDirection(Vector3 direction, out Vector3 slopeDirection)
    {
        slopeDirection = direction;

        // 发射射线检测前方是否有障碍物或斜面
        Vector3 origin = transform.position + new Vector3(0, 0.5f, 0); // 射线起始位置
        float distance = gridSize.magnitude; // 检测距离

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
