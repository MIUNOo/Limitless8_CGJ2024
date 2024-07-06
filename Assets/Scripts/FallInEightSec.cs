using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallInEightSec : MonoBehaviour
{
    public Vector3 direction = Vector3.down; // 移动方向
    public float interval = 0.5f;            // 移动间隔时间
    public float distance = 1f;              // 每次移动的距离

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = interval; // 初始化计时器为间隔时间
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime; // 减少计时器

        if (timer <= 0f)
        {
            transform.position += direction.normalized * distance; // 移动对象
            timer = interval; // 重置计时器
        }
    }
}
