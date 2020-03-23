using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Assets;
using System.Linq;

public class GridScripts2 : MonoBehaviour
{
    public GameObject backGround;
    public GameObject playGround;
    public int gridHeight;
    public int gridWidth;
    public List<GameObject> prefabs = new List<GameObject>();
    public List<string> types = new List<string>(7) { "I", "iL", "L", "O", "S", "T", "Z" };
    public List<List<Vector2>> patterns = new List<List<Vector2>>(7)
    {
        new List<Vector2>(4){new Vector2(0,0), new Vector2(1,0), new Vector2(2,0), new Vector2(3,0) },
        new List<Vector2>(4){new Vector2(0,0), new Vector2(0,-1), new Vector2(1,-1), new Vector2(2,-1) },
        new List<Vector2>(4){new Vector2(2,0), new Vector2(0,-1), new Vector2(1,-1), new Vector2(2,-1) },
        new List<Vector2>(4){new Vector2(0,0), new Vector2(1,0), new Vector2(0,-1), new Vector2(1,-1) },
        new List<Vector2>(4){new Vector2(1,0), new Vector2(2,0), new Vector2(0,-1), new Vector2(1,-1) },
        new List<Vector2>(4){new Vector2(1,0), new Vector2(0,-1), new Vector2(1,-1), new Vector2(2,-1) },
        new List<Vector2>(4){new Vector2(0,0), new Vector2(1,0), new Vector2(1,-1), new Vector2(2,-1) }
    };
    public List<Block> blocks = new List<Block>();
    public int startX;
    public int startY;
    public float currentTime = 0f;
    public float actionTime = 1f;
    public float defaultPeriod = 1f;
    public Block activeBlock;

    public float delayTime = 0.15f;
    public float holdTime = 0.15f;
    public float holdPeriod = 0.15f;

    // Start is called before the first frame update
    void Start()
    {
        playGround = GameObject.FindGameObjectWithTag("playGround");
        backGround = GameObject.FindGameObjectWithTag("backGround");
        startX = 4;
        startY = 12;

        DirectoryInfo dir = new DirectoryInfo("Assets/Resources/Prefabs2");
        FileInfo[] info = dir.GetFiles("*.prefab");
        foreach (FileInfo f in info)
        {
            GameObject obj = Resources.Load<GameObject>("Prefabs2/" + f.Name.Split('.')[0]);
            prefabs.Add(obj);
        }

        Block.grid = new bool[gridWidth, gridHeight];
        Block.gridWidth = gridWidth;
        Block.gridHeight = gridHeight;
    }

    Block CreateBlock()
    {
        List<GameObject> l = new List<GameObject>();
        int color = Random.Range(0, prefabs.Count);
        int t = Random.Range(0, patterns.Count);
        foreach(Vector2 v in patterns[t])
        {
            GameObject go = Instantiate(prefabs[color], playGround.transform);
            go.name = go.name.Replace("(Clone)", "");
            go.transform.position = new Vector3(startX, startY, 0) + new Vector3(v.x, v.y, 0);
            l.Add(go);
        }
        blocks.Add(new Block(l));
        return blocks[blocks.Count - 1];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            holdTime += Time.deltaTime;
            if (holdTime > delayTime)
            {
                delayTime += holdPeriod;
                if (activeBlock != null && Input.GetKey("a"))
                {
                    activeBlock.Move(new Vector3(-1, 0, 0));
                }
                if (activeBlock != null && Input.GetKey("d"))
                {
                    activeBlock.Move(new Vector3(1, 0, 0));
                }
                if (activeBlock != null && Input.GetKey("s"))
                {
                    activeBlock.Move(new Vector3(0, -1, 0));
                }
            }
        }
        else
        {
            delayTime = 0f;
            holdTime = 0f;
        }

        if (blocks.Any(b => b.state == true))
        {
            currentTime += Time.deltaTime;
            if (currentTime > actionTime)
            {
                actionTime += defaultPeriod;
                foreach (Block block in blocks)
                {
                    block.Move(new Vector3(0,-1,0));
                    block.CheckState();
                }
            }
        }
        else
        {
            activeBlock = CreateBlock();
        }
    }
}
