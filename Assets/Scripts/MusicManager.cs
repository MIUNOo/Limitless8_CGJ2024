using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    void Awake()
    {
        // ��鳡�����Ƿ��Ѿ�����MusicManagerʵ��
        if (FindObjectsOfType<MusicManager>().Length > 1)
        {
            Destroy(gameObject);  // ������ڣ������´�����ʵ��
        }
        else
        {
            DontDestroyOnLoad(gameObject);  // ��������ڣ����������ò�������
        }
    }
}
