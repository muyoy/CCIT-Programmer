using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewZombie : Monster
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Init(int line)
    {
        base.Init(line);
        HP = 220f;
        damage = 30f;
        //attackRange = 1f;
        attackDelay = 0.8f;
    }

}
