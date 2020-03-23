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
        public bool state;

        public Block(List<GameObject> objs)
        {
            pieces = new List<GameObject>(objs);
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

            SetGrid(true);
        }
    }
}
