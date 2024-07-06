using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallInEightSec : MonoBehaviour
{
    public Vector3 direction = Vector3.down; // 移动方向
    public float interval = 0.5f;            // 移动间隔时间
    public float distance = 1f;              // 每次移动的距离
    public LayerMask groundLayer;            // 地面层（用于检测）
    public LayerMask slopeLayer;             // 斜坡层（用于检测）
    public Vector3 boxSize = new Vector3(1, .5f, 1); // 盒子的尺寸

    private float timer;
    bool isOnSlope;

    void Start()
    {
        timer = interval; // 初始化计时器为间隔时间
    }

    void Update()
    {
        timer -= Time.deltaTime; // 减少计时器

        if (timer <= 0f)
        {
            CheckGrounded();

            if (!IsGrounded() && !IsOnSlope()&&!isOnSlope)
            {
                transform.position += direction.normalized * distance; // 移动对象
            }
            timer = interval; // 重置计时器
        }
    }

    bool IsGrounded()
    {
        RaycastHit hit;
        bool isGrounded = Physics.BoxCast(transform.position, boxSize / 2, direction, out hit, Quaternion.identity, distance, groundLayer);
        Debug.DrawLine(transform.position, transform.position + direction.normalized * distance, Color.green);
        Debug.Log("IsGrounded: " + isGrounded);
        return isGrounded;
    }

    bool IsOnSlope()
    {
        RaycastHit hit;
        bool isOnSlope = Physics.BoxCast(transform.position, boxSize / 2, direction, out hit, Quaternion.identity, distance, slopeLayer);
        Debug.DrawLine(transform.position, transform.position + direction.normalized * distance, Color.blue);
        Debug.Log("IsOnSlope: " + isOnSlope);
        return isOnSlope;
    }

    void CheckGrounded()
    {
        // 发射射线检测是否触底
        Vector3 origin = transform.position + new Vector3(0, -0.5f, 0); // 射线起始位置，可以根据你的模型调整
        Vector3 direction = Vector3.down;
        float distance = .5f; // 射线检测距离

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


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, Quaternion.identity, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero + direction * (distance / 2), boxSize);
    }
}
