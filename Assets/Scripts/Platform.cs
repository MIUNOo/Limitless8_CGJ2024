using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{

    /// <summary>
    /// �Զ����� �㶨����
    /// ײ��Wall��trap(��)��ֹͣ
    /// ����ʰȡ
    /// ��վ�� ���� ��������Ӱ��
    /// </summary>

    public float speed = 5f; // �ƶ��ٶ�
    public Vector3 moveDirection = Vector3.forward; // Ĭ���ƶ�����

    private Rigidbody rb;
    public bool isStopped = false;

    void Start()
    {
        // ��ȡRigidbody���
        rb = GetComponent<Rigidbody>();

        // ȡ��Rigidbody������Ӱ��
        rb.useGravity = false;

        // ����������ת�ᣬ���ַ���㶨
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void FixedUpdate()
    {
        if (!isStopped)
        {
            // ʹ��Rigidbody�ƶ�����
            rb.MovePosition(transform.position + moveDirection.normalized * speed * Time.fixedDeltaTime);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // ����Wall��Trapʱֹͣ
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Trap"))
        {
            isStopped = true;
        }
    }












}
