using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    protected float hp;
  
    public virtual float HP
    {
        get { return hp; }
        set
        {
            hp = value;
        }
    }
    public int lineNumber;          //현재 설치된 라인번호
    public bool isDead;

    public virtual void HpChanged(float _damage)
    {
        if (!isDead)
        {
            HP -= _damage;
        }
    }
    public Animator animator;

}