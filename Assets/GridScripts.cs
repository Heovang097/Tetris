using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScripts : MonoBehaviour
{
    public Transform[,] grid;
    public GameObject playGround;
    public GameObject[] prefabs;
    public int gridHeight;
    public int gridWidth;

    void createGrid()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        playGround = GameObject.FindGameObjectWithTag("playGround");
        gridWidth = (int)playGround.transform.localScale.x;
        gridHeight = (int)playGround.transform.localScale.y;
        grid = new Transform[gridWidth, gridHeight];
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
