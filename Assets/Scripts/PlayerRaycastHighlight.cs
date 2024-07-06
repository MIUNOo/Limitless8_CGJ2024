using UnityEngine;
using System.Collections.Generic;

public class PlayerRaycastHighlight : MonoBehaviour
{
    // 射线检测的最大距离
    public float rayDistance = 5.0f;
    // 高亮颜色
    public Color highlightColor = Color.red;
    // 默认颜色
    private Color defaultColor = Color.white;
    // 射线颜色
    public Color rayColor = Color.yellow;

    public LayerMask groundLayer;
    public LayerMask slopeLayer;

    // 储存所有高亮的对象
    private Dictionary<GameObject, int> highlightedObjects = new Dictionary<GameObject, int>();
    // 储存所有的射线对象
    private LineRenderer[] rayRenderers;

    void Start()
    {
        // 初始化LineRenderer数组
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
        // 定义方向
        Vector3[] directions = {
            transform.forward,  // 前
            -transform.forward, // 后
            transform.right,    // 右
            -transform.right,   // 左
            -transform.up       // 下
        };

        // 追踪新的高亮对象
        Dictionary<GameObject, int> newHighlightedObjects = new Dictionary<GameObject, int>();

        for (int i = 0; i < directions.Length; i++)
        {
            Vector3 direction = directions[i];
            Ray ray = new Ray(transform.position, direction);
            RaycastHit hit;

            // 设置射线的起点和终点
            rayRenderers[i].SetPosition(0, transform.position);
            rayRenderers[i].SetPosition(1, transform.position + direction * rayDistance);

            if (Physics.Raycast(ray, out hit, rayDistance))
            {
                // 获取射线击中的对象
                GameObject hitObject = hit.collider.gameObject;

                // 检查是否为Ground层或Slope层
                if (IsInLayerMask(hitObject, groundLayer) || IsInLayerMask(hitObject, slopeLayer))
                {
                    // 设置高亮
                    SetHighlightColor(hitObject);

                    // 缩短射线长度到击中点
                    rayRenderers[i].SetPosition(1, hit.point);

                    // 存储当前高亮对象
                    newHighlightedObjects[hitObject] = i;
                }
            }
        }

        // 重置不再被高亮的对象
        foreach (var obj in highlightedObjects.Keys)
        {
            if (!newHighlightedObjects.ContainsKey(obj))
            {
                ResetColor(obj);
            }
        }

        // 更新当前高亮对象
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
