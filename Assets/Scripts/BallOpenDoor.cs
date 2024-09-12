using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallOpenDoor : MonoBehaviour
{
    public GameObject door;
    bool interaction = true;


    // Update is called once per frame
    void Update()
    {
     
    } 
    private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag.Equals("Switch"))
            {
                if (interaction)
                {
                door.transform.Rotate(0, -90, 0);
                interaction = false;
            }

 
         
            }
        }
}
