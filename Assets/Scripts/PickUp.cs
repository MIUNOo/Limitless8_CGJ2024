using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public Transform holdPoint;  // ���������λ�ã�ͨ�����������ǰ��
    private GameObject heldItem;  // ��ǰ���е�����

    void Update()
    {
        // ����Ƿ��¿ո��
        if (Input.GetKeyDown(KeyCode.Space) && heldItem != null)
        {
            ReleaseItem();  // �ͷ�����
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (heldItem == null && other.gameObject.CompareTag("Pickup"))
        {
            PickupItem(other.gameObject);  // ʰȡ����
        }
    }

    void PickupItem(GameObject item)
    {
        heldItem = item;
        item.GetComponent<Rigidbody>().isKinematic = true;  // ����ΪKinematic����ֹ����Ӱ��
        item.transform.position = holdPoint.position;  // �ƶ������е�
        item.transform.parent = holdPoint;  // ��Ϊ���е���Ӷ���
    }

    void ReleaseItem()
    {
        if (heldItem != null)
        {
            heldItem.GetComponent<Rigidbody>().isKinematic = false;  // �ָ�����Ӱ��
            heldItem.transform.parent = null;  // ������ӹ�ϵ
            heldItem = null;
        }
    }
}
