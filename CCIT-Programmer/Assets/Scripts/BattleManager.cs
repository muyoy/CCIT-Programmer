using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public int milk = 0;
    public int monster_count;
    public float wave_time;
    public int wave_max;
    private int monster_max;

    private int Total_Dead;
    private float time_stamp;

    public bool[,] tower_check_grid = new bool[5, 9];
    public int[,] tower_grid = new int[5, 9];
    public Vector3[] line_start = new Vector3[5];
    public GameObject[] current_monster = new GameObject[3];

    Dictionary<int, Queue<GameObject>> monster_dic = new Dictionary<int, Queue<GameObject>>();

    Queue<GameObject> monsters_0 = new Queue<GameObject>();
    Queue<GameObject> monsters_1 = new Queue<GameObject>();
    Queue<GameObject> monsters_2 = new Queue<GameObject>();
    Queue<GameObject> monsters_3 = new Queue<GameObject>();
    Queue<GameObject> monsters_4 = new Queue<GameObject>();

    private float default_type = 0.0f, defaultUp_type = 0.0f;
    private int cur_wave_monster = 0;
    private int cur_wave = 0;

    public void Awake()
    {
        float x = 0.0f;
        for(int i = 0; i < 5; i++)
        {
            line_start[i] = new Vector3(x, 3.0f, 18.0f);
            for (int j = 0; j < 9; j++)
            {
                tower_check_grid[i, j] = false;
                tower_grid[i, j] = i;
            }
            x += 3.0f;
        }
        current_monster[0] = Resources.Load("Zombie_1", typeof(GameObject)) as GameObject;
        current_monster[1] = Resources.Load("Zombie_2", typeof(GameObject)) as GameObject;
        current_monster[2] = Resources.Load("Elemental", typeof(GameObject)) as GameObject;

        monster_dic.Add(0, monsters_0);
        monster_dic.Add(1, monsters_1);
        monster_dic.Add(2, monsters_2);
        monster_dic.Add(3, monsters_3);
        monster_dic.Add(4, monsters_4);
    }
    private void Start()
    {
        int default_type_max = 0, defaultUp_type_max = 0;
        for (int i = 0; i< wave_max; i++)
        {
            default_type = (float)Math.Round(default_type * 1.05f, MidpointRounding.AwayFromZero) + 0.7f;
            default_type_max += (int)default_type;
        }
        for (int i = 0; i < wave_max - 5; i++)
        {
            defaultUp_type = (float)Math.Round(defaultUp_type * 1.05f, MidpointRounding.AwayFromZero) + 0.7f;
            defaultUp_type_max += (int)defaultUp_type;
        }

        monster_max = default_type_max + defaultUp_type_max + 2;

        default_type = 0.0f;
        defaultUp_type = 0.0f;

        StartCoroutine(Wave_Loop());
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
        int line = UnityEngine.Random.Range(0, 5);
        monster_dic[line].Enqueue(monster);
        monster = Instantiate(monster, line_start[line], Quaternion.Euler(0.0f, -180.0f, 0.0f));
    }

    public void MonsterDead(bool isdead)
    {

    }
    
    public GameObject SetTargetMonster(int line)
    {
        return monster_dic[line].Peek();
    }

    public int Next_Wave(bool clear)
    {
        default_type = (float)Math.Round(default_type * 1.05f, MidpointRounding.AwayFromZero) + 0.7f;
        if (cur_wave >= 5)
        {
            defaultUp_type = (float)Math.Round(defaultUp_type * 1.05f, MidpointRounding.AwayFromZero) + 0.7f;
        }
        return cur_wave_monster = (int)default_type + (int)defaultUp_type;
    }

    //private void Update()
    //{

    //}

    private IEnumerator Wave_Loop()
    {
        int monster_0 = 0;
        int monster_1 = 0;
        while (cur_wave <= wave_max)
        {
            Next_Wave(true);
            while(monster_count < cur_wave_monster)
            {
                if (cur_wave <= 5)
                {
                    SetMonsterLine(current_monster[0]);
                    monster_0++;
                }
                else
                {
                    int mon = UnityEngine.Random.Range(0, 2);
                    if (mon == 0 && monster_0 < default_type)
                    {
                        SetMonsterLine(current_monster[0]);
                        monster_0++;
                    }
                    else if (mon == 1 && monster_1 < defaultUp_type)
                    {
                        SetMonsterLine(current_monster[1]);
                        monster_1++;
                    }
                }
                monster_count = monster_0 + monster_1;
                yield return new WaitForSeconds(5.0f);
            }

            yield return null;
        }
    }

    private void OnDisable()
    {
        monsters_0.Clear();
        monsters_1.Clear();
        monsters_2.Clear();
        monsters_3.Clear();
        monsters_4.Clear();

        monster_dic.Clear();
    }
}