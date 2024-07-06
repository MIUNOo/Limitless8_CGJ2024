using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Player player;
    public Text limitedUI;
    public GameObject cam;

    bool isLimited;

    int limitedTurn;

    // Start is called before the first frame update
    void Start()
    {
        limitedTurn = int.Parse(limitedUI.text);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)||Input.GetKeyDown(KeyCode.A)||Input.GetKeyDown(KeyCode.S)||Input.GetKeyDown(KeyCode.D))
        {
            limitedTurn--;
            limitedUI.text = limitedTurn.ToString(); 
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            cam.transform.Rotate(0, 0, -90);
            isLimited = true;
            Time.timeScale = 0;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            cam.transform.Rotate(0, 0, 90);
            isLimited = false;
            Time.timeScale = 1;
        }
    }



}
