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

    [SerializeField] protected float armorClass = 5f;         //방어도

    protected override void Awake()
    {
        type = Type.Defender;
    }


    protected override void Work()
    {
        base.Work();
    }

    public override void HpChanged(float _damage)
    {
        HP -= _damage * armorClass * 0.01f;
    }

}
