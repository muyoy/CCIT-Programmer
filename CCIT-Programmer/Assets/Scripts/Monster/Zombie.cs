using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Monster
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Init()
    {
        base.Init();
        damage = 30f;
        attackRange = 1f;
        attackDelay = 0.8f;
    }

}
