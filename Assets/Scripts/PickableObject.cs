using System;
using UnityEngine;
using static UnityEditor.Progress;

public class PickableObject : MonoBehaviour
{
    //public Transform playerTransform; // Reference to the player's transform
    //public Transform mapTransform; // Reference to the map's transform
    //public float pickUpForce = 10f; // Force used to pull the object towards the player
    //public float dropForce = 5f; // Force used to drop the object
    //public float pickUpDistance = 2f; // Distance at which the object will be picked up
    //public float smoothFollowSpeed = 5f; // Speed for smooth following
    //private bool isPickedUp = false; // Whether the object is currently picked up
    //private Rigidbody rb; // Reference to the Rigidbody component
    //private Collider col; // Reference to the Collider component

    //void Start()
    //{
    //    rb = GetComponent<Rigidbody>(); // Cache the Rigidbody component
    //    col = GetComponent<Collider>(); // Cache the Collider component
    //}

    //void Update()
    //{
    //    if (isPickedUp)
    //    {
    //        // Apply a force to pull the object towards the player
    //        Vector3 targetPosition = playerTransform.position;
    //        Vector3 directionToPlayer = (targetPosition - transform.position).normalized;
    //        rb.AddForce(directionToPlayer * pickUpForce, ForceMode.Acceleration);

    //        // Smooth the movement using Lerp
    //        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothFollowSpeed);

    //        if (Input.GetKeyDown(KeyCode.F))
    //        {
    //            Drop();
    //        }
    //    }
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("player") && !isPickedUp)
    //    {
    //        Pickup();
    //    }
    //}

    //private void Pickup()
    //{
    //    isPickedUp = true;

    //    // Disable gravity and set mass to a very low value for easier manipulation
    //    if (rb != null)
    //    {
    //        rb.useGravity = false;
    //        rb.mass = 0.1f;
    //        rb.velocity = Vector3.zero; // Clear any existing velocity
    //        rb.angularVelocity = Vector3.zero; // Clear any existing angular velocity
    //    }

    //    // Disable the Collider to prevent unwanted collisions
    //    if (col != null)
    //    {
    //        col.enabled = false;
    //    }

    //    // Attach the object to the player
    //    transform.SetParent(playerTransform);
    //    transform.localPosition = Vector3.zero; // Align with player
    //    transform.localRotation = Quaternion.identity; // Align the rotation

    //    Debug.Log("Picked up the object.");
    //}

    //private void Drop()
    //{
    //    isPickedUp = false;

    //    // Reset mass and re-enable gravity
    //    //if (rb != null)
    //    //{
    //    //    rb.useGravity = true;
    //    //    rb.mass = 1f; // Reset to default mass, adjust as needed

    //    //    // Optionally apply a force downward to simulate dropping
    //    //    // rb.AddForce(Vector3.down * dropForce, ForceMode.Impulse);
    //    //}

    //    // Re-enable the Collider to restore collisions
    //    if (col != null)
    //    {
    //        col.enabled = true;
    //    }

    //    // Attach the object to the map and optionally adjust its position
    //    transform.SetParent(mapTransform);

    //    Debug.Log("Dropped the object.");
    //}

    public Transform holdPoint;  // 持有物体的位置，通常设置在玩家前方
    private GameObject heldItem;  // 当前持有的物体

    [SerializeField]
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
    }

    void Update()
    {
        // 检查是否按下空格键
        if (Input.GetKeyDown(KeyCode.Space) && heldItem != null)
        {

            ReleaseItem();  // 释放物体
        }
        //if (Input.GetKeyDown("t"))
        //{
        //    Debug.Log("check " + heldItem.transform.position+" "+ heldItem.name+ heldItem.GetHashCode() + " player pos " + transform.position);
        //}
    }

    void OnTriggerEnter(Collider other)
    {
        if (heldItem == null && other.gameObject.CompareTag("Pickable"))
        {
            PickupItem(other.gameObject);  // 拾取物体
        }
    }

    void PickupItem(GameObject item)
    {
        Debug.Log("pickup " + item.transform.position);
        heldItem = item;
        item.GetComponent<Rigidbody>().isKinematic = true;  // 设置为Kinematic，防止物理影响
        item.transform.position = holdPoint.position;  // 移动到持有点
        item.transform.parent = holdPoint;  // 成为持有点的子对象


        //停止平台
        if (item.TryGetComponent(out Platform platform))
        {
            platform.isStopped = true;
        }

    }

    void ReleaseItem()
    {
        //Debug.Log("ReleaseItem0 " + heldItem.transform.position);
        if (!gameManager.IsMapHorizontal())
        {
            heldItem.GetComponent<Rigidbody>().isKinematic = false;  // 恢复物理影响
                                                                     //停止平台
            if (heldItem.TryGetComponent(out Platform platform))
            {
                platform.isStopped = false;
            }
        }
        //Debug.Log("ReleaseItem1 " + heldItem.transform.position);
        heldItem.transform.SetParent(null, true);



        //Debug.Log("ReleaseItem2 " + heldItem.transform.position);
        //heldItem.transform.position = holdPoint.transform.position;
        heldItem = null;
    }

}
