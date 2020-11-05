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
    /* 배틀매니저 게임 오브젝트 */
    BattleManager BMGameObject;
    /* 배틀매니저 스크립트 컴포넌트 */
    // BattleManager battleManager;

    #region Monster_Status
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
    protected const float MONSTER_ATTACK_REACH = 3f;
    protected float injuredHP;
    protected float moribundHP;
    #endregion Monster Property

    #region Tower Property
    protected GameObject firstTower;
    protected Tower towerInfo;
    protected GameObject tower;
    #endregion Tower Property

    #region Coroutines
    Coroutine AttackCoroutine;
    Coroutine WalkingCoroutine;
    Coroutine HpCheckCoroutine;
    Coroutine DistanceCheckCoroutine;
    #endregion

    protected virtual void Awake()
    {
        monsterRb = gameObject.GetComponent<Rigidbody>();
        animator = gameObject.GetComponent<Animator>();

        idleID = Animator.StringToHash("Idle");
        walkID = Animator.StringToHash("Walk");
        attackID = Animator.StringToHash("Attack");
        deadID = Animator.StringToHash("Dead");
        hpID = Animator.StringToHash("HP"); // HP에 따라 조정

        BMGameObject = GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleManager>();

        injuredHP = HP * 0.7f;
        moribundHP = HP * 0.3f;

    }

    public IEnumerator Start_On(int line)
    {
        Init(line);
        WalkMonster();// Test

        yield return null;
    }

    public override void HpChanged (float _damage){
        if (!isDead)
        {
            HP -= _damage;
            HpCheck();
        }
    }

    protected virtual void Init(int line)
    {
        HP = 180f;
        damage = 30f;
        speed = 1f;
        //attackRange = 1f;
        attackDelay = 0.8f;

        lineNumber = line;

        isBlocked = false;
        firstTower = null; // Tower GameObject Init

        isDead = false;
    }

    public void WalkMonster()
    {
        if (WalkingCoroutine == null)
            WalkingCoroutine = StartCoroutine(WalkingForward());
    }

    public IEnumerator WalkingForward() // 몬스터가 앞으로 전진
    {
        //animator.SetTrigger(walkID);
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
            //animator.SetFloat(hpID, MONSTER_STATE_NORMAL);
            animator.SetFloat(hpID, MONSTER_STATE_NORMAL);
        }
        else if (hp < injuredHP && hp >= moribundHP)
        {
            //animator.SetFloat(hpID, MONSTER_STATE_INJURED);
            animator.SetFloat(hpID, MONSTER_STATE_INJURED);
        }
        else if (hp > 0 && hp < moribundHP)
        {
            //animator.SetFloat(hpID, MONSTER_STATE_MORIBUND);
            animator.SetFloat(hpID, MONSTER_STATE_MORIBUND);
        }
        else if (hp <= 0)
        {
            StopCoroutine(WalkingCoroutine);
            WalkingCoroutine = null;
            //animator.SetTrigger(deadID);
            animator.SetTrigger(deadID);
            isDead = true;

            // 배틀 매니저에게 몬스터가 죽으면 플래그 세우기
            BMGameObject.MonsterDead(lineNumber);
        }
    }

    protected IEnumerator DistanceCheck()
    {
        do // 타워에 의해 몬스터가 길이 막혀있는 동안
        {
            try
            {
                if (firstTower == null)
                {
                    firstTower = tower;
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
                    if (WalkingCoroutine == null)
                    {
                        WalkingCoroutine = StartCoroutine(WalkingForward());
                    }
                }
                
            }catch(NullReferenceException ex)
            {
                Console.WriteLine("Distance Check Reference null : " + ex.Message);
            }
            yield return null;

        } while (isBlocked);
    }

    protected IEnumerator Attack()
    {
        do
        {
            try
            {
                animator.SetTrigger(attackID);
                animatorClipInfo = animator.GetCurrentAnimatorClipInfo(0);
                if (animatorClipInfo[0].weight >= 1)
                {
                    animator.Play(attackID);
                    if (towerInfo.HP <= 0)
                    {
                        if (WalkingCoroutine == null)
                        {
                            WalkingCoroutine = StartCoroutine(WalkingForward());
                        }
                        yield break;
                    }
                }
            }catch(NullReferenceException ex)
            {
                Console.WriteLine("Attack Reference null : " + ex.Message);
            }

            yield return null;
        } while (true);
    }

    protected void Hit()
    {
        towerInfo.HpChanged((int)damage);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Tower"))
        {
            towerInfo = other.GetComponent<Tower>();
            tower = other.gameObject;
            DistanceCheckCoroutine = StartCoroutine(DistanceCheck());
        }
    }


}

