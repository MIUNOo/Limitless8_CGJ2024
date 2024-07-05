using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float moveSpeed = 5.0f;  // 移动速度，用于平滑移动
    public Vector3 gridSize = new Vector3(1, 0, 1);  // 网格大小，假设是1x1

    private Vector3 targetPosition;
    private bool isMoving;

    void Start()
    {
        // 初始化目标位置为角色当前位置
        targetPosition = transform.position;
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

        if (direction != Vector3.zero)
        {
            // 更新目标位置
            targetPosition += Vector3.Scale(direction, gridSize);
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
    //    // 如果角色在地面上
    //    if (controller.isGrounded)
    //    {
    //        // 获取玩家输入
    //        float moveHorizontal = Input.GetAxis("Horizontal");
    //        float moveVertical = Input.GetAxis("Vertical");

    //        // 将输入转换为角色方向
    //        Vector3 move = new Vector3(moveHorizontal, 0.0f, moveVertical);

    //        // 设置移动方向（角色朝向）
    //        moveDirection = transform.TransformDirection(move) * speed;

    //        // 跳跃
    //        if (Input.GetButton("Jump"))
    //        {
    //            moveDirection.y = jumpSpeed;
    //        }
    //    }

    //    // 应用重力
    //    moveDirection.y -= gravity * Time.deltaTime;

    //    // 移动角色
    //    controller.Move(moveDirection * Time.deltaTime);

    //    // 更新摄像机位置
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




    //public float moveStep = 1.0f; // 移动步长
    //public float rotationStep = 10.0f; // 旋转步长（角度）

    //private void Update()
    //{
    //    // 移动处理
    //    if (Input.GetKeyDown(KeyCode.W))
    //    {
    //        // 向前移动
    //        this.transform.position += this.transform.forward * moveStep;
    //    }
    //    if (Input.GetKeyDown(KeyCode.S))
    //    {
    //        // 向后移动
    //        this.transform.position -= this.transform.forward * moveStep;
    //    }
    //    if (Input.GetKeyDown(KeyCode.A))
    //    {
    //        // 向左移动
    //        this.transform.position -= this.transform.right * moveStep;
    //    }
    //    if (Input.GetKeyDown(KeyCode.D))
    //    {
    //        // 向右移动
    //        this.transform.position += this.transform.right * moveStep;
    //    }

    //    // 旋转处理
    //    if (Input.GetKeyDown(KeyCode.Q))
    //    {
    //        // 向左旋转
    //        this.transform.Rotate(Vector3.up, -rotationStep);
    //    }
    //    if (Input.GetKeyDown(KeyCode.E))
    //    {
    //        // 向右旋转
    //        this.transform.Rotate(Vector3.up, rotationStep);
    //    }
    //}

    //public float moveStep = 1.0f; // 移动步长
    //public float rotationStep = 10.0f; // 旋转步长（角度）
    //public float gravity = 9.81f; // 重力
    //public LayerMask groundLayer; // 地面层，用于检测地面法线

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
    //        // 捕捉输入并计算移动
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

    //        // 保持角色直立
    //        AlignToGround();
    //    }

    //    // 添加重力
    //    moveDirection.y -= gravity * Time.deltaTime;

    //    // 进行移动
    //    _controller.Move(moveDirection * Time.deltaTime);
    //}

    //private void HandleRotation()
    //{
    //    // 旋转处理
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
    //    // 从角色中心向下投射光线
    //    if (Physics.Raycast(transform.position, Vector3.down, out hit, _controller.height / 2 + 0.5f, groundLayer))
    //    {
    //        // 根据地面法线调整角色的旋转
    //        Vector3 forward = Vector3.Cross(transform.right, hit.normal);
    //        this.transform.rotation = Quaternion.LookRotation(forward, hit.normal);
    //    }
    //}


}
