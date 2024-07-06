using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5.0f;  // 移动速度
    public Vector3 gridSize = new Vector3(1, 1, 1);  // 网格大小
    public LayerMask groundLayer;  // 地面层，用于检测坡道
    public LayerMask obstacleLayer;  // 障碍物层，用于检测墙
    public float slopeForce = 1.5f;  // 在坡道上移动的力度
    public float slopeRayLength = 1.5f;  // 检测坡道的射线长度
    public float obstacleRayLength = 1.0f;  // 检测障碍物的射线长度

    private Vector3 targetPosition;
    private bool isMoving;
    private CharacterController characterController;

    void Start()
    {
        // 初始化目标位置为角色当前位置
        targetPosition = RoundToGrid(transform.position);
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // 处理输入，只有当角色不在移动时才接受新输入
        if (!isMoving)
        {
            HandleInput();
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
            // 检测前方是否有障碍物或坡道
            if (!IsObstacleInDirection(direction))
            {
                // 更新目标位置
                targetPosition += Vector3.Scale(direction, gridSize);
                isMoving = true;
            }
        }
    }

    void MoveToTarget()
    {
        if (isMoving)
        {
            // 获取平滑移动的方向
            Vector3 moveDirection = (targetPosition - transform.position).normalized * moveSpeed;

            // 检测是否在坡道上
            if (OnSlope())
            {
                moveDirection += Vector3.down * slopeForce;
            }

            // 移动角色
            characterController.Move(moveDirection * Time.deltaTime);

            // 当到达目标位置时，停止移动
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                transform.position = targetPosition;  // 强制设置位置以防止漂移
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

        // 在目标方向上投射射线
        if (Physics.Raycast(origin, direction, out hit, obstacleRayLength, obstacleLayer))
        {
            return true;  // 检测到障碍物
        }
        return false;  // 没有检测到障碍物
    }

    Vector3 RoundToGrid(Vector3 position)
    {
        return new Vector3(
            Mathf.Round(position.x / gridSize.x) * gridSize.x,
            Mathf.Round(position.y / gridSize.y) * gridSize.y,
            Mathf.Round(position.z / gridSize.z) * gridSize.z
        );
    }

    //public float moveSpeed = 5.0f;  // 移动速度
    //public Vector3 gridSize = new Vector3(1, 1, 1);  // 网格大小
    //public LayerMask groundLayer;  // 地面层，用于检测坡道
    //public float slopeForce = 1.5f; // 在坡道上移动的力度
    //public float slopeRayLength = 1.5f;  // 射线长度，用于检测坡道

    //private Vector3 targetPosition;
    //private bool isMoving;
    //private CharacterController characterController;

    //void Start()
    //{
    //    // 初始化目标位置为角色当前位置
    //    targetPosition = RoundToGrid(transform.position);
    //    characterController = GetComponent<CharacterController>();
    //}

    //void Update()
    //{
    //    // 处理输入，只有当角色不在移动时才接受新输入
    //    if (!isMoving)
    //    {
    //        HandleInput();
    //    }

    //    // 平滑移动到目标位置
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
    //        // 更新目标位置
    //        targetPosition += Vector3.Scale(direction, gridSize);
    //        isMoving = true;
    //    }
    //}

    //void MoveToTarget()
    //{
    //    if (isMoving)
    //    {
    //        // 获取平滑移动的方向
    //        Vector3 moveDirection = (targetPosition - transform.position).normalized * moveSpeed;

    //        // 检测是否在坡道上
    //        if (OnSlope())
    //        {
    //            moveDirection += Vector3.down * slopeForce;
    //        }

    //        // 移动角色
    //        characterController.Move(moveDirection * Time.deltaTime);

    //        // 当到达目标位置时，停止移动
    //        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
    //        {
    //            transform.position = targetPosition;  // 强制设置位置以防止漂移
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
    ///// 能用的普通移动 不能处理上坡 不带character 
    ///// </summary>
    //public float moveSpeed = 5.0f;  // 移动速度，用于平滑移动
    //public Vector3 gridSize = new Vector3(1, 0, 1);  // 网格大小，假设是1x1

    //private Vector3 targetPosition;
    //private bool isMoving;

    //void Start()
    //{
    //    // 初始化目标位置为角色当前位置
    //    targetPosition = transform.position;
    //}

    //void Update()
    //{
    //    // 处理输入，只有当角色不在移动时才接受新输入
    //    if (!isMoving)
    //    {
    //        HandleInput();
    //    }

    //    // 平滑移动到目标位置
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
    //        // 更新目标位置
    //        targetPosition += Vector3.Scale(direction, gridSize);
    //        isMoving = true;
    //    }
    //}

    //void MoveToTarget()
    //{
    //    // 如果目标位置和当前位置不同，移动角色
    //    if (isMoving)
    //    {
    //        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

    //        // 当到达目标位置时，停止移动
    //        if (transform.position == targetPosition)
    //        {
    //            isMoving = false;
    //        }
    //    }
    //}




}
