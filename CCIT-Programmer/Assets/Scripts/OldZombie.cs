using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldZombie : Zombie
{
    protected override void Start()
    {
        base.Start();
        HP = 180f;
    }


}
