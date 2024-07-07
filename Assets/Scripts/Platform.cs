using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{

    /// <summary>
    /// 自动运行 恒定方向
    /// 撞到Wall和trap(刺)会停止
    /// 可以拾取
    /// 可站立 悬浮 不受重力影响
    /// </summary>

    public float speed = 5f; // 移动速度
    public Vector3 moveDirection = Vector3.forward; // 默认移动方向

    private Rigidbody rb;
    public bool isStopped = false;

    void Start()
    {
        // 获取Rigidbody组件
        rb = GetComponent<Rigidbody>();

        // 取消Rigidbody的重力影响
        rb.useGravity = false;

        // 锁定所有旋转轴，保持方向恒定
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void FixedUpdate()
    {
        if (!isStopped)
        {
            // 使用Rigidbody移动物体
            rb.MovePosition(transform.position + moveDirection.normalized * speed * Time.fixedDeltaTime);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // 碰到Wall或Trap时停止
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Trap"))
        {
            isStopped = true;
        }
    }












}
