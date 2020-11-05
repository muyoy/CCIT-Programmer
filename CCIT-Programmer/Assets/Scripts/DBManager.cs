using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class DBStruct
{
    public enum TowerType { Product, Short, Long, Explosion, Shield };

    [Serializable]
    public class TowerData
    {
        public int num;
        public string name;
        public TowerType type;
        public int cost;
        public float hp;
        public float coolTime;
        public float atkSpeed;
        public float atk;
        public float atkRange;
        public string description;

        public TowerData(int num, string name, TowerType type, int cost, float hp, float coolTime, float atkSpeed, float atk, float atkRange, string description)
        {
            this.num = num;
            this.name = name;
            this.type = type;
            this.cost = cost;
            this.hp = hp;
            this.coolTime = coolTime;
            this.atkSpeed = atkSpeed;
            this.atk = atk;
            this.atkRange = atkRange;
            this.description = description;
        }

        public TowerData(string[] data)
        {
            int count = 0;
            this.num = int.Parse(data[count++]);
            this.name = data[count++];
            this.type = (TowerType)Enum.Parse(typeof(TowerType), data[count++]);
            this.cost = int.Parse(data[count++]);
            this.hp = float.Parse(data[count++]);
            this.coolTime = float.Parse(data[count++]);
            this.atkSpeed = float.Parse(data[count++]);
            this.atk = float.Parse(data[count++]);
            this.atkRange = float.Parse(data[count++]);
            this.description = data[count++];
        }
    }

}


public class DBManager : MonoBehaviour
{
    public List<DBStruct.TowerData> towerDatas = new List<DBStruct.TowerData>();

    void Start()
    {
        LoadTowerDB("/TowerDB.txt");
    }

    public void LoadTowerDB(string path)
    {
        string[] towerDB = File.ReadAllLines(Application.streamingAssetsPath + path);
        for (int i = 1; i < towerDB.Length; i++)
        {
            towerDatas.Add(new DBStruct.TowerData(towerDB[i].Split(',')));
        }


    }

}
