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

    public Transform holdPoint;  // ���������λ�ã�ͨ�����������ǰ��
    private GameObject heldItem;  // ��ǰ���е�����

    [SerializeField]
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
    }

    void Update()
    {
        // ����Ƿ��¿ո��
        if (Input.GetKeyDown(KeyCode.Space) && heldItem != null)
        {

            ReleaseItem();  // �ͷ�����
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
            PickupItem(other.gameObject);  // ʰȡ����
        }
    }

    void PickupItem(GameObject item)
    {
        Debug.Log("pickup " + item.transform.position);
        heldItem = item;
        item.GetComponent<Rigidbody>().isKinematic = true;  // ����ΪKinematic����ֹ����Ӱ��
        item.transform.position = holdPoint.position;  // �ƶ������е�
        item.transform.parent = holdPoint;  // ��Ϊ���е���Ӷ���


        //ֹͣƽ̨
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
            heldItem.GetComponent<Rigidbody>().isKinematic = false;  // �ָ�����Ӱ��
                                                                     //ֹͣƽ̨
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
