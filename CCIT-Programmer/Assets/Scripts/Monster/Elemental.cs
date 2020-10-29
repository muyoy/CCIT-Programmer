using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elemental : Monster
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Init()
    {
        hp = 250f;
        damage = 35f;
        speed = 1.3f;
        attackRange = 1f;
        attackDelay = 1.3f;

        isBlocked = false;
        firstTower = null; // Tower GameObject Init

        isDead = false;
    }
}
