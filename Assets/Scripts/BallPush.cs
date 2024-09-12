using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPush : MonoBehaviour
{
    private Rigidbody rb;
    public float pushForce = 5f;  // �����Ĵ�С

    void Start()
    {
        rb = GetComponent<Rigidbody>();  // ��ȡRigidbody���
    }

    void OnCollisionEnter(Collision collision)
    {
        // �����ײ�Ķ����Ƿ��С���ҡ��򡰵��ˡ��ı�ǩ
        if (collision.gameObject.tag == "player" || collision.gameObject.tag == "enemy")
        {
            Vector3 pushDirection = collision.contacts[0].normal * -1;  // ������������
            collision.rigidbody.AddForce(pushDirection * pushForce, ForceMode.Impulse);  // ����ײ�����������
            rb.velocity = Vector3.zero;  // ������ٶ�����Ϊ0��ʹ��ͣ��ԭ��
        }
    }
}
