using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public Transform holdPoint;  // 持有物体的位置，通常设置在玩家前方
    private GameObject heldItem;  // 当前持有的物体

    void Update()
    {
        // 检查是否按下空格键
        if (Input.GetKeyDown(KeyCode.Space) && heldItem != null)
        {
            ReleaseItem();  // 释放物体
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (heldItem == null && other.gameObject.CompareTag("Pickup"))
        {
            PickupItem(other.gameObject);  // 拾取物体
        }
    }

    void PickupItem(GameObject item)
    {
        heldItem = item;
        item.GetComponent<Rigidbody>().isKinematic = true;  // 设置为Kinematic，防止物理影响
        item.transform.position = holdPoint.position;  // 移动到持有点
        item.transform.parent = holdPoint;  // 成为持有点的子对象
    }

    void ReleaseItem()
    {
        if (heldItem != null)
        {
            heldItem.GetComponent<Rigidbody>().isKinematic = false;  // 恢复物理影响
            heldItem.transform.parent = null;  // 解除父子关系
            heldItem = null;
        }
    }
}
