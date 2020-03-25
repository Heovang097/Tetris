using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manage : MonoBehaviour
{
    public GameObject currentGame;
    GameObject game;
    // Start is called before the first frame update
    void Start()
    {
        game = Instantiate(currentGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void CreateNewGame()
    {
        Debug.Log("Yeah");
    }
}
