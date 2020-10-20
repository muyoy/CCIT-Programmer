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
        Damage = 30f;
        AttackRange = 1f;
        AttackDelay = 0.8f;
    }

}
