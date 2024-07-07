using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public float speed = 1.0f;  // �����ƶ��ٶ�
    private Rigidbody rb;  // Rigidbody�������

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        MoveForward();  // ��ʼ�ƶ�
    }

    void Update()
    {
        // ȷ������ʼ�����趨�ٶ�ǰ��
        rb.velocity = transform.forward * speed;

        // ���� X �� Z �����ת��ֻ���� Y ��������ת
        Vector3 currentRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0, currentRotation.y, 0);
    }

    void OnCollisionEnter(Collision collision)
    {
        // ����Ƿ���ײ����ǽ
        if (collision.gameObject.tag == "Wall")
        {
            TurnAround();  // ��ͷ
        }
    }

    void MoveForward()
    {
        rb.velocity = transform.forward * speed;
    }

    void TurnAround()
    {
        transform.Rotate(0, 180, 0);  // ��y����ת180��
    }
}
