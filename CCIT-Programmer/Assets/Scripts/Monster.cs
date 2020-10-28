///
/// EDITOR : Kim han gyul
/// Script : Monster Parent Script
/// Last EDITED : 2020-10-28
///


using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro;
using UnityEngine;

public class Monster : MonoBehaviour
{
    #region Monster_Status
    [SerializeField]
    protected float HP;
    [SerializeField]
    protected float Damage;
    [SerializeField]
    protected float Speed; // 몬스터 이동 속도
    [SerializeField]
    protected float AttackRange; // 공격 사거리
    [SerializeField]
    protected float AttackDelay; // 공격 쿨타임
    #endregion Monster_Status

    protected bool isBlocked; // 몬스터가 타워에 의해 길이 막혔는가?
    protected Rigidbody monsterRb; 
    protected Animator animator;

    protected float TowerHP;

    public bool imDead; // 사망 플래그

    #region Coroutines
    Coroutine AttackCoroutine;
    Coroutine WalkingCoroutine;
    #endregion



    protected virtual void Awake()
    {
        monsterRb = gameObject.GetComponent<Rigidbody>();
        animator = gameObject.GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        HP = 180f;
        Damage = 30f;
        Speed = 2f;
        AttackRange = 1f;
        AttackDelay = 0.8f;

        isBlocked = false;
        imDead = false;

        StartCoroutine(Dead()); // 사망 판정 코루틴

        WalkingCoroutine = StartCoroutine(WalkingForward()); // Test
    }

    public IEnumerator WalkingForward() // 몬스터가 앞으로 전진
    {
        animator.SetBool("Walk", true);
        while (!isBlocked)
        {
            monsterRb.position += Vector3.forward * Speed * Time.deltaTime;

            yield return null;
        }
    }

    protected void OnTriggerEnter(Collider other) // 몬스터가 타워와 부딫혔을 때
    {
        if (other.gameObject.CompareTag("Tower"))
        {
            StopCoroutine(WalkingForward()); // 몬스터 움직임 정지
            WalkingCoroutine = null;

            //if(AttackCoroutine == null)
            //{
            //    Tower tower = other.gameObject.GetComponent<Tower>();
            //    AttackCoroutine = StartCoroutine(Attack(tower.HP)); // 타워에게 데미지 계산
            //}
            isBlocked = true;
            animator.SetBool("Attack", true); // 몬스터 공격 판단 True

        }
        else
        {
            // 타워에 부딫히지 않음
        }
        
    }

    protected void OnTriggerExit(Collider other)
    {
        //if (other.gameObject.tag.Equals("Tower"))
        //{
        //    if (other.gameObject.GetComponent<Tower>().HP <= 0) // 타워의 체력이 0 이하일 경우 다시 걷기 시작
        //    {
        //        StopCoroutine(Attack(0));
        //        AttackCoroutine = null;
        //        animator.SetBool("Attack", false);
        //        WalkingCoroutine = StartCoroutine(WalkingForward()); // 몬스터 다시 움직임
        //    }
        //}
    }

    protected IEnumerator Attack(float towerHP) // 몬스터의 공격 모션
    {
        if(towerHP == 0)
        {
            yield return null;
        }
        while (isBlocked)
        {
            towerHP -= Damage;

            yield return new WaitForSeconds(AttackDelay); // AttackDelay 마다 Damage만큼 towerHP 감소
        }
    }

    protected IEnumerator Dead()
    {
        while (true)
        {
            if(HP <= 0)
            {
                StopAllCoroutines();
                animator.SetBool("Death", true);

                imDead = true; // 캐릭터 사망
            }

            yield return null;
        }
    }

    protected IEnumerator DistanceCheck()
    {
        while (isBlocked)
        {

            yield return new WaitForFixedUpdate();

        }
    }

}

