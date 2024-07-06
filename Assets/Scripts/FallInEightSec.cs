using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallInEightSec : MonoBehaviour
{
    public Vector3 direction = Vector3.down; // �ƶ�����
    public float interval = 0.5f;            // �ƶ����ʱ��
    public float distance = 1f;              // ÿ���ƶ��ľ���

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = interval; // ��ʼ����ʱ��Ϊ���ʱ��
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime; // ���ټ�ʱ��

        if (timer <= 0f)
        {
            transform.position += direction.normalized * distance; // �ƶ�����
            timer = interval; // ���ü�ʱ��
        }
    }
}
