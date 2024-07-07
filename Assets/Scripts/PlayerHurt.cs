using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHurt : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip breakSound;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = breakSound;
        audioSource.playOnAwake = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 检查碰撞对象是否为“敌人”
        if (collision.gameObject.tag == "Enemy"|| collision.gameObject.tag == "Trap")
        {
            audioSource.Play();  // 播放音效
            Invoke("ResetGame", 2f);  // 延时调用重置场景的函数
        }
    }

    void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);  // 重新加载当前场景
    }
}
