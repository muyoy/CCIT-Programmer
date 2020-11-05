// ==============================================================
// 타워
// 
//
//  AUTHOR: Yang Se Eun
// CREATED: 2020-10-21
// UPDATED: 2020-10-21
//===============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Character
{
    public enum Type {Productor, Attacker, Defender };

    //data
    protected Type type;
    [SerializeField] protected float maxHp = 20f;

    public override float HP
    {
        get { return hp; }
        set
        {
            hp = value;
            hp = Mathf.Clamp(value, 0, maxHp);

            if(!isDead && hp<=0)
            {
                hp = -1;
                Dead();
            }
        }
    }



    [SerializeField] protected float originOffset = 0.35f;
    [SerializeField] protected Vector3 startingPos;



    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        //나중에 지울것
        //BattleManager가 호출
        StartCoroutine(Start_On(2));
    }


    public override void HpChanged(float _damage)
    {
        base.HpChanged(_damage);
    }

    /// <summary>
    /// 상태 시작 (스폰되면 함수 호출하기)
    /// </summary>
    /// <returns></returns>
    public IEnumerator Start_On(int _lineNum)
    {
        Init(_lineNum);

        Work();
        yield return null;
    }

    protected virtual void Work()
    {
    }

    protected virtual void Dead()
    {
        isDead = true;

        SetAnimation("Dead",true);
        Debug.Log(transform.name + " is Dead");
    }

   

    protected void SetAnimation(string _AnimTrigger)
    {
        animator.SetTrigger(_AnimTrigger);
    }
    protected void SetAnimation(string _AnimBool,bool trueFalse)
    {
        animator.SetBool(_AnimBool, trueFalse);
    }

    private void Init(int _lineNum)
    {
        lineNumber = _lineNum;
        hp = maxHp;
        startingPos = transform.position + transform.TransformDirection(Vector3.up) * originOffset;
    }



}
