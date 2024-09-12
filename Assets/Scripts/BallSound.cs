using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSound : MonoBehaviour
{
    private AudioSource audioSource;
    private bool isMoving = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // ������Ƿ����ƶ����ٶȴ���һ����ֵ��
        if (GetComponent<Rigidbody>().velocity.magnitude > 0.1f)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
            audioSource.Stop();  // ֹͣ������Ƶ
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // ��������ƶ�״̬��������Ƶδ�ڲ���ʱ��ײ
        if (isMoving && !audioSource.isPlaying)
        {
            audioSource.Play();  // ������Ƶ
        }
    }
}
