// ==============================================================
// 방어형 타워
// 
//
//  AUTHOR: Yang Se Eun
// CREATED: 2020-10-21
// UPDATED: 2020-10-21
//===============================================================


using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Defender : Tower
{

    [SerializeField] protected float def = 5f;         //방어력

    protected override void Awake()
    {
        type = Type.Defender;
    }


    protected override void Work()
    {
        base.Work();
    }

    public override float HpChange(int _damage)
    {
        return Hp -= (_damage * def * 0.01f);
    }

}
