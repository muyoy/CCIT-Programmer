using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public bool[,] tower_check_grid = new bool[5, 9];
    public int[,] tower_grid = new int[5, 9];
    public Vector3[] line_start = new Vector3[5];

    Queue<GameObject> monsters_0 = new Queue<GameObject>();
    Queue<GameObject> monsters_1 = new Queue<GameObject>();
    Queue<GameObject> monsters_2 = new Queue<GameObject>();
    Queue<GameObject> monsters_3 = new Queue<GameObject>();
    Queue<GameObject> monsters_4 = new Queue<GameObject>();


    public void Awake()
    {

        for(int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                tower_check_grid[i, j] = false;
                tower_grid[i, j] = i;
            }
        }

    }

    public bool Tower_Check(GameObject tower)
    {
        int x, z;
        x = (int)tower.gameObject.transform.localPosition.x;
        z = (int)tower.gameObject.transform.localPosition.z;

        if(tower_check_grid[x / 3, z / 3] == true)
        {
            return false;
        }
        else
        {
            tower_check_grid[x / 3, z / 3] = true;
            return true;
        }
    }

    public void SetMonsterLine(GameObject monster)
    {
        int line = Random.Range(0, 4);

        switch(line)
        {
            case 0:
                monsters_0.Enqueue(monster);
                monster.transform.position = line_start[0];
                break;
            case 1:
                monsters_1.Enqueue(monster);
                break;
            case 2:
                monsters_2.Enqueue(monster);
                break;
            case 3:
                monsters_3.Enqueue(monster);
                break;
            case 4:
                monsters_4.Enqueue(monster);
                break;
        }
    }

    //public GameObject SetTargetMonster(int line)
    //{

    //}

    private void Update()
    {
        
    }
}