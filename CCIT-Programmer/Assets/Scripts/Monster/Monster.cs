///
/// EDITOR : Kim han gyul
/// Script : Monster Parent Script
/// Created : 2020 - 10 - ..
/// Last EDITED : 2020-10-29
///


using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Resources;
using TMPro;
using UnityEditor;
using UnityEngine;

public class Monster : Character
{
    #region Monster_Status
    [SerializeField]
    protected float hp;
    [SerializeField]
    protected float damage;
    [SerializeField]
    protected float speed; // 몬스터 이동 속도
    [SerializeField]
    protected float attackRange; // 공격 사거리
    [SerializeField]
    protected float attackDelay; // 공격 쿨타임
    #endregion Monster_Status

    #region Monster Property
    protected bool isBlocked; // 몬스터가 타워에 의해 길이 막혔는가?
    protected Rigidbody monsterRb;
    protected Animation monsterAnimation;
    protected Animator animator;
    #region Animator Property
    protected int idleID;
    protected int attackID;
    protected int walkID;
    protected int hpID;
    protected int deadID;
    protected AnimatorClipInfo[] animatorClipInfo;
    protected const float MONSTER_STATE_NORMAL = 0;
    protected const float MONSTER_STATE_INJURED = 1;
    protected const float MONSTER_STATE_MORIBUND = 2;
    #endregion Animator Property
    protected float distance; // 타워와 좀비 사이의 거리
    protected const float MONSTER_ATTACK_REACH = 1f;
    protected float injuredHP;
    protected float moribundHP;
    #endregion Monster Property

    #region Tower Property
    protected GameObject firstTower;
    protected Character towerInfo;
    #endregion Tower Property

    #region Coroutines
    Coroutine AttackCoroutine;
    Coroutine WalkingCoroutine;
    Coroutine HpCheckCoroutine;
    Coroutine DistanceCheckCoroutine;
    #endregion

    public override float HP
    {
        get { return hp; }
        set { 
            hp = value;
            HpCheck();
        }
    }

    protected virtual void Awake()
    {
        monsterRb = gameObject.GetComponent<Rigidbody>();
        animator = gameObject.GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        idleID = Animator.StringToHash("Idle");
        walkID = Animator.StringToHash("Walk");
        attackID = Animator.StringToHash("Attack");
        deadID = Animator.StringToHash("Dead");
        hpID = Animator.StringToHash("HP"); // HP에 따라 조정

        injuredHP = hp * 0.7f;
        moribundHP = hp * 0.3f;

        StartCoroutine(Start_On(1));
    }

    public IEnumerator Start_On(int line)
    {
        Init(line);
        WalkMonster();// Test

        yield return null;
    }

    protected virtual void Init(int line)
    {
        hp = 180f;
        damage = 30f;
        speed = 2f;
        attackRange = 1f;
        attackDelay = 0.8f;
        lineNumber = line;
        isBlocked = false;
        firstTower = null; // Tower GameObject Init

        isDead = false;
    }

    public override void HpChanged(float _damage)
    {
        hp -= _damage;
        HP = hp;
    }

    public void WalkMonster()
    {
        if (WalkingCoroutine == null)
            WalkingCoroutine = StartCoroutine(WalkingForward());
    }

    public IEnumerator WalkingForward() // 몬스터가 앞으로 전진
    {
        animator.SetTrigger(walkID);
        while (!isBlocked)
        {
            monsterRb.position += Vector3.back * speed * Time.deltaTime;

            yield return null;
        }
    }

    protected void HpCheck()
    {
        if (hp >= injuredHP)
        {
            animator.SetFloat(hpID, MONSTER_STATE_NORMAL);
        }
        else if (hp < injuredHP && hp >= moribundHP)
        {
            animator.SetFloat(hpID, MONSTER_STATE_INJURED);
        }
        else if(hp>0 && hp<moribundHP)
        {
            animator.SetFloat(hpID, MONSTER_STATE_MORIBUND);
        }else if (hp <= 0)
        {
            StopCoroutine(WalkingCoroutine);
            WalkingCoroutine = null;
            animator.SetTrigger(deadID);
            isDead = true;
        }
    }

    protected IEnumerator DistanceCheck(Collider other)
    {
        do // 타워에 의해 몬스터가 길이 막혀있는 동안
        {
            if (firstTower == null)
            {
                firstTower = other.gameObject;
            }

            distance = Vector3.Distance(gameObject.transform.position, firstTower.transform.position); // Monster 와 Tower의 거리 계산
            if (distance <= MONSTER_ATTACK_REACH)
            {
                isBlocked = true;
                StopCoroutine(WalkingCoroutine);
                WalkingCoroutine = null;
                StartCoroutine(Attack());
                yield break;
            }
            else
            {
                isBlocked = false; // 계속 전진
            }

            yield return new WaitForFixedUpdate();

        } while (isBlocked);
    }

    protected IEnumerator Attack()
    {
        do
        {
            animator.SetTrigger(attackID);
            animatorClipInfo = animator.GetCurrentAnimatorClipInfo(0);
            if (animatorClipInfo[0].weight == 1)
            {
                monsterAnimation.Play();
                if (towerInfo.HP <= 0)
                {
                    if(WalkingCoroutine == null)
                    {
                        WalkingCoroutine = StartCoroutine(WalkingForward());
                    }
                    yield break;
                }
            }
        } while (true);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tower"))
        {
            towerInfo = other.GetComponent<Character>();
            DistanceCheckCoroutine = StartCoroutine(DistanceCheck(other));
        }
    }

    
}

