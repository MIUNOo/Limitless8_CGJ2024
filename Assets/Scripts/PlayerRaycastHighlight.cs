using UnityEngine;
using System.Collections.Generic;

public class PlayerRaycastHighlight : MonoBehaviour
{
    // ���߼���������
    public float rayDistance = 5.0f;
    // ������ɫ
    public Color highlightColor = Color.red;
    // Ĭ����ɫ
    private Color defaultColor = Color.white;
    // ������ɫ
    public Color rayColor = Color.yellow;

    public LayerMask groundLayer;
    public LayerMask slopeLayer;

    // �������и����Ķ���
    private Dictionary<GameObject, int> highlightedObjects = new Dictionary<GameObject, int>();
    // �������е����߶���
    private LineRenderer[] rayRenderers;

    void Start()
    {
        // ��ʼ��LineRenderer����
        rayRenderers = new LineRenderer[5];
        for (int i = 0; i < rayRenderers.Length; i++)
        {
            GameObject rayObject = new GameObject("RayRenderer" + i);
            rayObject.transform.SetParent(this.transform);
            LineRenderer lr = rayObject.AddComponent<LineRenderer>();
            lr.startWidth = 0.05f;
            lr.endWidth = 0.05f;
            lr.material = new Material(Shader.Find("Unlit/Color"));
            lr.material.color = rayColor;
            rayRenderers[i] = lr;
        }
    }

    void Update()
    {
        // ���巽��
        Vector3[] directions = {
            transform.forward,  // ǰ
            -transform.forward, // ��
            transform.right,    // ��
            -transform.right,   // ��
            -transform.up       // ��
        };

        // ׷���µĸ�������
        Dictionary<GameObject, int> newHighlightedObjects = new Dictionary<GameObject, int>();

        for (int i = 0; i < directions.Length; i++)
        {
            Vector3 direction = directions[i];
            Ray ray = new Ray(transform.position, direction);
            RaycastHit hit;

            // �������ߵ������յ�
            rayRenderers[i].SetPosition(0, transform.position);
            rayRenderers[i].SetPosition(1, transform.position + direction * rayDistance);

            if (Physics.Raycast(ray, out hit, rayDistance))
            {
                // ��ȡ���߻��еĶ���
                GameObject hitObject = hit.collider.gameObject;

                // ����Ƿ�ΪGround���Slope��
                if (IsInLayerMask(hitObject, groundLayer) || IsInLayerMask(hitObject, slopeLayer))
                {
                    // ���ø���
                    SetHighlightColor(hitObject);

                    // �������߳��ȵ����е�
                    rayRenderers[i].SetPosition(1, hit.point);

                    // �洢��ǰ��������
                    newHighlightedObjects[hitObject] = i;
                }
            }
        }

        // ���ò��ٱ������Ķ���
        foreach (var obj in highlightedObjects.Keys)
        {
            if (!newHighlightedObjects.ContainsKey(obj))
            {
                ResetColor(obj);
            }
        }

        // ���µ�ǰ��������
        highlightedObjects = newHighlightedObjects;
    }

    void SetHighlightColor(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = highlightColor;
        }
    }

    void ResetColor(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = defaultColor;
        }
    }

    bool IsInLayerMask(GameObject obj, LayerMask layerMask)
    {
        return (layerMask.value & (1 << obj.layer)) > 0;
    }
}
