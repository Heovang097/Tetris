using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace Assets
{
    public class Block
    {
        public static int gridHeight;
        public static int gridWidth;
        public static bool[,] grid;
        public List<GameObject> pieces;
        public Vector2 pivot;
        public bool state;

        public Block(List<GameObject> objs, Vector2 pivot)
        {
            pieces = new List<GameObject>(objs);
            this.pivot = pivot;
            SetGrid(true);
            state = true;
        }

        public void SetGrid(bool value)
        {
            foreach (GameObject go in pieces)
            {
                grid[(int)go.transform.position.x, (int)go.transform.position.y] = value;
            }
        }

        public bool IsCollided(Vector3 v)
        {
            bool flag = false;

            SetGrid(false);

            foreach (GameObject go in pieces)
                if (go.transform.position.y + v.y <= 0 || go.transform.position.x + v.x < 0 || go.transform.position.x + v.x >= gridWidth || grid[(int)go.transform.position.x + (int)v.x, (int)go.transform.position.y + (int)v.y] == true)
                    flag = true;

            SetGrid(true);

            return flag;
        }

        public void CheckState()
        {
            if (IsCollided(new Vector3(0, -1, 0)))
                state = false;
        }

        public void Move(Vector3 v)
        {
            if (IsCollided(v) == true)
                return;

            SetGrid(false);

            foreach (GameObject go in pieces)
                go.transform.position = go.transform.position + v;
            pivot += new Vector2(v.x, v.y);

            SetGrid(true);
        }

        public void Print()
        {
            Debug.Log(pieces[0].transform.position.x + " " + pieces[0].transform.position.y);
            Debug.Log(pieces[1].transform.position.x + " " + pieces[1].transform.position.y);
            Debug.Log(pieces[2].transform.position.x + " " + pieces[2].transform.position.y);
            Debug.Log(pieces[3].transform.position.x + " " + pieces[3].transform.position.y);
        }

        public void Rotate()
        {
            SetGrid(false);
            //Print();
            //Debug.Log("pivot " + pivot.x + " " + pivot.y);

            List<Vector2> newPositions = new List<Vector2>();
            bool flag = true;

            foreach(GameObject go in pieces)
            {
                Vector2 v = new Vector2(go.transform.position.x, go.transform.position.y) - pivot;
                Vector2 newV = new Vector2(-v.y + pivot.x, v.x + pivot.y);
                if (newV.y <= 0 || newV.x < 0 || newV.x >= gridWidth || grid[(int)newV.x, (int)newV.y])
                    flag = false;
                newPositions.Add(newV);
            }
            if (flag == true)
                for (int i = 0; i < pieces.Count; i++)
                    pieces[i].transform.position = new Vector3(newPositions[i].x, newPositions[i].y, 0);

            //Print();
            SetGrid(true);
        }

        public void Remove(Vector2 v)
        {
            GameObject go = pieces.First(p => p.transform.position.x == v.x && p.transform.position.y == v.y);
            int index = pieces.IndexOf(go);
            GameObject.Destroy(go);
            //pieces.Remove(pieces.First(p => p.transform.position.x == v.x && p.transform.position.y == v.y));
            pieces.RemoveAt(index);
            grid[(int)v.x, (int)v.y] = false;
        }
    }
}
