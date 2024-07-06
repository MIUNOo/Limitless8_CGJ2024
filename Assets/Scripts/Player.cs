using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5.0f;  // 移动速度，用于平滑移动
    public Vector3 gridSize = new Vector3(1, 0, 1);  // 网格大小，假设是1x1
    public float fallInterval = .5f;  // 每隔多久下落一格
    public LayerMask groundLayer;  // 用于射线检测的层

    private Vector3 targetPosition;
    private bool isMoving;
    private float fallTimer;
    private bool isGrounded;

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
        // 检测前方是否有障碍物
        Vector3 target = targetPosition + Vector3.Scale(direction, gridSize);
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
    }

    bool IsObstacleInDirection(Vector3 direction, Vector3 target)
    {
        // 发射射线检测前方是否有障碍物
        Vector3 origin = transform.position + new Vector3(0, 0.5f, 0); // 射线起始位置
        float distance = .8f; // 检测距离

        // 使用盒体检测来覆盖整个方块区域
        if (Physics.Raycast(origin, direction, distance, groundLayer))
        {
            return true;
        }
        return false;
    }
}