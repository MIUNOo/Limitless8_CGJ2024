using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallInEightSec : MonoBehaviour
{
    public Vector3 direction = Vector3.down; // �ƶ�����
    public float interval = 0.5f;            // �ƶ����ʱ��
    public float distance = 1f;              // ÿ���ƶ��ľ���
    public LayerMask groundLayer;            // ����㣨���ڼ�⣩
    public LayerMask slopeLayer;             // б�²㣨���ڼ�⣩
    public Vector3 boxSize = new Vector3(1, .5f, 1); // ���ӵĳߴ�

    private float timer;
    bool isOnSlope;

    void Start()
    {
        timer = interval; // ��ʼ����ʱ��Ϊ���ʱ��
    }

    void Update()
    {
        timer -= Time.deltaTime; // ���ټ�ʱ��

        if (timer <= 0f)
        {
            CheckGrounded();

            if (!IsGrounded() && !IsOnSlope()&&!isOnSlope)
            {
                transform.position += direction.normalized * distance; // �ƶ�����
            }
            timer = interval; // ���ü�ʱ��
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
        // �������߼���Ƿ񴥵�
        Vector3 origin = transform.position + new Vector3(0, -0.5f, 0); // ������ʼλ�ã����Ը������ģ�͵���
        Vector3 direction = Vector3.down;
        float distance = .5f; // ���߼�����

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


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, Quaternion.identity, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero + direction * (distance / 2), boxSize);
    }
}
