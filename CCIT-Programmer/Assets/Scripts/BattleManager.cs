using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public int monster_count;
    public float monster_spawn_delay;
    public int wave_max;
    public float monster_max;

    private float Total_Dead = 0.0f;
    public float wave_Dead;

    private float default_type = 0.0f, defaultUp_type = 0.0f;
    public int cur_wave_monster = 0;
    public int cur_wave = 1;

    private bool isWaveClear = true;
    private bool isRush = false;

    public bool[,] tower_check_grid = new bool[5, 9];
    public int[,] tower_grid = new int[5, 9];
    public Vector3[] line_start = new Vector3[5];
    public List<GameObject> tower = new List<GameObject>();
    public GameObject[] current_monster = new GameObject[3];
    private Coroutine wave = null;
    public BattleUI battleui;

    private const int monster_tag = -1139050900;
    #region Dictionary, Queue
    Dictionary<int, Queue<GameObject>> monster_dic = new Dictionary<int, Queue<GameObject>>();

    Queue<GameObject> monsters_0 = new Queue<GameObject>();
    Queue<GameObject> monsters_1 = new Queue<GameObject>();
    Queue<GameObject> monsters_2 = new Queue<GameObject>();
    Queue<GameObject> monsters_3 = new Queue<GameObject>();
    Queue<GameObject> monsters_4 = new Queue<GameObject>();

    #endregion

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
        current_monster[0] = Resources.Load("Monster/Zombie_1", typeof(GameObject)) as GameObject;
        current_monster[1] = Resources.Load("Monster/Zombie_2", typeof(GameObject)) as GameObject;
        current_monster[2] = Resources.Load("Monster/Elemental", typeof(GameObject)) as GameObject;

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
            default_type = (float)Math.Round(default_type * 1.05f + 0.7f, MidpointRounding.AwayFromZero);
            default_type_max += (int)default_type;
        }
        for (int i = 0; i < wave_max - 5; i++)
        {
            defaultUp_type = (float)Math.Round(defaultUp_type * 1.05f + 0.7f, MidpointRounding.AwayFromZero);
            defaultUp_type_max += (int)defaultUp_type;
        }
        
        monster_max = default_type_max + defaultUp_type_max + 2;

        default_type = 0.0f;
        defaultUp_type = 0.0f;
    }

    public int Tower_Line_Check(GameObject tower)
    {
        int grid_num;
        int x, z;
        grid_num = int.Parse(tower.gameObject.transform.parent.name);
        x = grid_num / 9;
        z = grid_num % 9;
        AddTower(tower);
        tower_check_grid[x, z] = true;
        return x;
    }

    private void AddTower(GameObject _tower)
    {
        tower.Add(_tower);
    }

    private void Grid(GameObject _tower)
    {
        int grid_num;
        int x, z;
        grid_num = int.Parse(_tower.gameObject.transform.parent.name);
        x = grid_num / 9;
        z = grid_num % 9;

        tower_check_grid[x, z] = false;
    }

    public void RemoveTower(GameObject _tower)
    {
        Grid(_tower);
        Destroy(_tower);
        tower.Remove(_tower);
    }

    private void Update()
    {

        if (isWaveClear)
            wave = StartCoroutine(Wave_Loop());

        if (wave_Dead >= cur_wave_monster)
        {
            Next_Wave();
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            MonsterDead(1);
        }

        for(int i =0; i < tower.Count; i++)
        {
            if(tower[i].GetComponent<Tower>().isDead == true)
            {
                RemoveTower(tower[i]);
                break;
            }
        }
    }

    #region Monster_Instan
    public void SetMonsterLine(GameObject monster)
    {
        //int line = UnityEngine.Random.Range(0, 5);
        int line = 1; //test용
        GameObject _monster = Instantiate(monster, line_start[line], Quaternion.Euler(0.0f, -180.0f, 0.0f));
        if(monster == current_monster[2])
        {
            _monster.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
        }
        StartCoroutine(_monster.GetComponent<Monster>().Start_On(line));
        monster_dic[line].Enqueue(_monster);
    }

    public void MonsterDead(int monster_line)
    {
        GameObject deadmonster = monster_dic[monster_line].Peek();
        Destroy(deadmonster);
        monster_dic[monster_line].Dequeue();

        wave_Dead++;
        Total_Dead++;

        battleui.SetRoundProgress(monster_max, Total_Dead);

        if (Total_Dead == monster_max)
        {
            Debug.Log("Win");
            Time.timeScale = 0;
        }
    }
    
    public GameObject SetTargetMonster(int line)
    {
        if(monster_dic[line].Count <= 0)
        {
            return null;
        }
        return monster_dic[line].Peek();
    }

    #endregion

    #region Wave_Function

    private int Next_Wave_cal()
    {
        default_type = (float)Math.Round(default_type * 1.05f + 0.7f, MidpointRounding.AwayFromZero);
        if (cur_wave >= 5)
        {
            defaultUp_type = (float)Math.Round(defaultUp_type * 1.05f + 0.7f, MidpointRounding.AwayFromZero);
        }
        isWaveClear = false;
        return cur_wave_monster = (int)default_type + (int)defaultUp_type;
    }

    private void Next_Wave()
    {
        if (wave != null)
        {
            StopCoroutine(wave);
            wave = null;
        }
        cur_wave++;
        if(cur_wave == wave_max/2 || cur_wave == wave_max)
        {
            isRush = true;
        }
        wave_Dead = 0;
        isWaveClear = true;
    }

    private IEnumerator Wave_Loop()
    {
        int monster_0 = 0;
        int monster_1 = 0;
        if(cur_wave <= wave_max && isWaveClear)
        {
            Next_Wave_cal();
            while(monster_count < cur_wave_monster )
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
                        if (monster_0 == default_type)
                        {
                            monster_0 = (int)default_type;
                            SetMonsterLine(current_monster[1]);
                            monster_1++;
                        }
                        else
                        {
                            SetMonsterLine(current_monster[0]);
                            monster_0++;
                        }
                    }
                    else if (mon == 1 && monster_1 < defaultUp_type)
                    {
                        if (monster_1 == defaultUp_type)
                        {
                            monster_1 = (int)defaultUp_type;
                            SetMonsterLine(current_monster[0]);
                            monster_0++;
                        }
                        else
                        {
                            SetMonsterLine(current_monster[1]);
                            monster_1++;
                        }
                    }
                }

                if (isRush)
                {
                    yield return new WaitForSeconds(monster_spawn_delay);
                    SetMonsterLine(current_monster[2]);
                    monster_count = monster_0 + monster_1 + 1;
                    isRush = false;
                }
                else
                {
                    monster_count = monster_0 + monster_1;
                }
                yield return new WaitForSeconds(monster_spawn_delay);
            }
        }
    }

    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.GetHashCode() == monster_tag)
        {
            Time.timeScale = 0;
            Debug.Log("Lose");
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