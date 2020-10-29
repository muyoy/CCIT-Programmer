using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elemental : Monster
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Init(int line)
    {
        hp = 450f;
        damage = 35f;
        speed = 1.5f;
        //attackRange = 1f;
        attackDelay = 1.3f;
        lineNumber = line;

        isBlocked = false;
        firstTower = null; // Tower GameObject Init

        isDead = false;
    }
}
