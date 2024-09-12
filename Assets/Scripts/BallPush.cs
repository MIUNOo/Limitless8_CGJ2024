using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPush : MonoBehaviour
{
    private Rigidbody rb;
    public float pushForce = 5f;  // 推力的大小

    void Start()
    {
        rb = GetComponent<Rigidbody>();  // 获取Rigidbody组件
    }

    void OnCollisionEnter(Collision collision)
    {
        // 检查碰撞的对象是否有“玩家”或“敌人”的标签
        if (collision.gameObject.tag == "player" || collision.gameObject.tag == "enemy")
        {
            Vector3 pushDirection = collision.contacts[0].normal * -1;  // 计算推力方向
            collision.rigidbody.AddForce(pushDirection * pushForce, ForceMode.Impulse);  // 给碰撞对象添加推力
            rb.velocity = Vector3.zero;  // 将球的速度设置为0，使其停在原地
        }
    }
}
