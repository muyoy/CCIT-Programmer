using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public bool[,] tower_check_grid = new bool[5,9];
    public int[,] tower_grid = new int[5,9];
    public void Awake()
    {
        for(int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                tower_check_grid[i, j] = false;
            }
        }
    }

    bool Tower_Check(GameObject tower)
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

    private void Update()
    {
        
    }
}
