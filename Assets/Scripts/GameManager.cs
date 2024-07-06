using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Player player;
    public Text limitedUI;

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
    }
}
