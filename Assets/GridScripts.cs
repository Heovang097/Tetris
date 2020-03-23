using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GridScripts : MonoBehaviour
{
    public GameObject backGround;
    public GameObject playGround;
    public int gridHeight;
    public int gridWidth;
    public bool[,] grid;
    public List<GameObject> prefabs = new List<GameObject>();
    public int startX;
    public int startY;
    public float currentTime = 0f;
    public float actionTime = 1f;
    public float period = 1f;
    public GameObject activeBlock = null;

    Vector3 GetTopLeft(GameObject go)
    {
        if (go.name == "I-block")
            return new Vector3(go.transform.position.x - 2, go.transform.position.y + (float)0.5, 0);
        else if (go.name == "Invert-L-block" || go.name == "L-block" || go.name == "S-block" || go.name == "Z-block" || go.name == "T-block")
            return new Vector3(go.transform.position.x - (float)1.5, go.transform.position.y + 1, 0);
        else if (go.name == "O-block")
            return new Vector3(go.transform.position.x - 1, go.transform.position.y + 1, 0);
        return new Vector3(0,0,0);
    }

    Vector3 GetPosition(Vector3 tl, GameObject go)
    {
        if (go.name == "I-block")
            return new Vector3(tl.x + 2, tl.y - (float)0.5, 0);
        else if (go.name == "Invert-L-block" || go.name == "L-block" || go.name == "S-block" || go.name == "Z-block" || go.name == "T-block")
            return new Vector3(tl.x + (float)1.5, tl.y - 1, 0);
        else if (go.name == "O-block")
            return new Vector3(tl.x + 1, tl.y - 1, 0);
        return new Vector3(0, 0, 0);
    }

    List<Vector2> GetCellBlockedByBlock(Vector3 tl, GameObject go)
    {
        List<Vector2> l = new List<Vector2>();
        Vector2[] arr = new Vector2[4];

        if (go.name == "I-block")
            arr = new Vector2[4] { new Vector2(tl.x, tl.y), new Vector2(tl.x + 1, tl.y), new Vector2(tl.x + 2, tl.y), new Vector2(tl.x + 3, tl.y) };
        else if (go.name == "Invert-L-block")
            arr = new Vector2[4] { new Vector2(tl.x, tl.y), new Vector2(tl.x, tl.y - 1), new Vector2(tl.x + 1, tl.y - 1), new Vector2(tl.x + 2, tl.y - 1) };
        else if (go.name == "L-block")
            arr = new Vector2[4] { new Vector2(tl.x, tl.y - 1), new Vector2(tl.x + 1, tl.y - 1), new Vector2(tl.x + 2, tl.y - 1), new Vector2(tl.x + 2, tl.y) };
        else if (go.name == "O-block")
            arr = new Vector2[4] { new Vector2(tl.x, tl.y), new Vector2(tl.x + 1, tl.y), new Vector2(tl.x, tl.y - 1), new Vector2(tl.x + 1, tl.y - 1) };
        else if (go.name == "S-block")
            arr = new Vector2[4] { new Vector2(tl.x + 1, tl.y), new Vector2(tl.x + 2, tl.y), new Vector2(tl.x, tl.y - 1), new Vector2(tl.x + 1, tl.y - 1) };
        else if (go.name == "T-block")
            arr = new Vector2[4] { new Vector2(tl.x + 1, tl.y), new Vector2(tl.x, tl.y - 1), new Vector2(tl.x + 1, tl.y - 1), new Vector2(tl.x + 2, tl.y - 1) };
        else if (go.name == "Z-block")
            arr = new Vector2[4] { new Vector2(tl.x, tl.y), new Vector2(tl.x + 1, tl.y), new Vector2(tl.x + 1, tl.y - 1), new Vector2(tl.x + 2, tl.y - 1) };

        foreach (Vector2 v in arr)
        {
            //Debug.Log(v.x + " " + v.y);
            l.Add(v);
        }

        return l;
    }

    bool IsBlocked(GameObject go)
    {
        List<Vector2> l = GetCellBlockedByBlock(GetTopLeft(go) + new Vector3(0,-1,0), go);
        foreach (Vector2 v in l)
            if (v.y <= 0 || grid[(int)v.x, (int)v.y])
                return true;
        return false;
    }

    void MovingBlock(GameObject go)
    {
        List<Vector2> lBefore = GetCellBlockedByBlock(GetTopLeft(go), go);
        foreach (Vector2 v in lBefore)
            grid[(int)v.x, (int)v.y] = false;
        go.transform.position = go.transform.position + new Vector3(0, -1, 0);
        List<Vector2> lAfter = GetCellBlockedByBlock(GetTopLeft(go), go);
        foreach (Vector2 v in lAfter)
            grid[(int)v.x, (int)v.y] = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        playGround = GameObject.FindGameObjectWithTag("playGround");
	    backGround = GameObject.FindGameObjectWithTag("backGround");
    	grid = new bool[gridWidth, gridHeight];
        startX = 4;
        startY = 12;

        DirectoryInfo dir = new DirectoryInfo("Assets/Resources/Prefabs");
	    FileInfo[] info = dir.GetFiles("*.prefab");
	    foreach(FileInfo f in info)
	    {
		    GameObject obj = Resources.Load<GameObject>("Prefabs/" + f.Name.Split('.')[0]);
            obj.transform.position = GetPosition(new Vector3(startX, startY, 0), obj);
            prefabs.Add(obj);
	    }

    }

    // Update is called once per frame
    void Update()
    {
        if (activeBlock != null)
        {
            currentTime += Time.deltaTime;
            if (currentTime > actionTime)
            {
                if (!IsBlocked(activeBlock))
                {
                    actionTime += period;
                    MovingBlock(activeBlock);
                }
                else
                {
                    activeBlock = null;
                    currentTime = 0f;
                    actionTime = period;
                }
            }
        }
        else
        {
            int tmp = Random.Range(0, prefabs.Count);
            activeBlock = Instantiate(prefabs[0], playGround.transform); //****************************************
            activeBlock.name = activeBlock.name.Replace("(Clone)", "");
        }
    }
}
