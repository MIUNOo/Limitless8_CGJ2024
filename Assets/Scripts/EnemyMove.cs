using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public float speed = 1.0f;  // 敌人移动速度
    private Rigidbody rb;  // Rigidbody组件引用

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        MoveForward();  // 初始移动
    }

    void Update()
    {
        // 确保敌人始终以设定速度前进
        rb.velocity = transform.forward * speed;

        // 锁定 X 和 Z 轴的旋转，只允许 Y 轴自由旋转
        Vector3 currentRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0, currentRotation.y, 0);
    }

    void OnCollisionEnter(Collision collision)
    {
        // 检查是否碰撞到了墙
        if (collision.gameObject.tag == "Wall")
        {
            TurnAround();  // 调头
        }
    }

    void MoveForward()
    {
        rb.velocity = transform.forward * speed;
    }

    void TurnAround()
    {
        transform.Rotate(0, 180, 0);  // 沿y轴旋转180度
    }
}
