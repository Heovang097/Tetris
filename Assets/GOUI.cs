using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOUI : MonoBehaviour
{
    public Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
