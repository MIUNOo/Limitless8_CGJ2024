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
        // 检测球是否在移动（速度大于一定阈值）
        if (GetComponent<Rigidbody>().velocity.magnitude > 0.1f)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
            audioSource.Stop();  // 停止播放音频
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // 如果球在移动状态，并且音频未在播放时碰撞
        if (isMoving && !audioSource.isPlaying)
        {
            audioSource.Play();  // 播放音频
        }
    }
}
