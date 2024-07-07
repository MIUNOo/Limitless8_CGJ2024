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
        // �����ײ�����Ƿ�Ϊ�����ˡ�
        if (collision.gameObject.tag == "Enemy"|| collision.gameObject.tag == "Trap")
        {
            audioSource.Play();  // ������Ч
            Invoke("ResetGame", 2f);  // ��ʱ�������ó����ĺ���
        }
    }

    void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);  // ���¼��ص�ǰ����
    }
}
