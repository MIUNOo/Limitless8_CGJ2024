using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    void Awake()
    {
        // 检查场景中是否已经存在MusicManager实例
        if (FindObjectsOfType<MusicManager>().Length > 1)
        {
            Destroy(gameObject);  // 如果存在，销毁新创建的实例
        }
        else
        {
            DontDestroyOnLoad(gameObject);  // 如果不存在，保留并设置不被销毁
        }
    }
}
