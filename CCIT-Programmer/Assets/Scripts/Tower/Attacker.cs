﻿// ==============================================================
// 공격형 타워 구조
// 
//
//  AUTHOR: Yang Se Eun
// CREATED: 2020-10-21
// UPDATED: 2020-10-21
//===============================================================


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : Tower
{
    #region parameter

    private float nodeSize = 3.0f;
    private bool inAttackRange = false;
    protected GameObject targetObj = null;
    private BattleManager battlemanager;

    //Stats
    [Header("Stats")]
    private int atk = 10;
    [SerializeField] protected int atkRange = 1;
    [SerializeField] protected float atkCooltime = 1.0f;

    #endregion

    protected override void Awake()
    {
        base.Awake();
        type = Type.Attacker;
        battlemanager = GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleManager>();
    }

    protected override void Work()
    {
        base.Work();

        StartCoroutine(CheckMonster());
    }

    //Animation Event Function
    public virtual void AttackOn()
    {
        if(targetObj !=null)
        {
            Debug.Log("Tower Attack! " + atk);
            targetObj.GetComponent<Character>().HpChanged(atk);
        }
    }

    protected virtual void Attack()
    {
    }

    private IEnumerator CheckMonster()
    {
        while(!isDead)
        {
            targetObj = battlemanager.SetTargetMonster(lineNumber);
            inAttackRange = false;


            if ((targetObj != null) && (targetObj.transform.position - transform.position).magnitude <= atkRange * nodeSize)
            {
                inAttackRange = true;
                Attack();
                yield return new WaitForSeconds(atkCooltime);
            }
            else
            {
                Idle();
            }
            yield return null;
        }
    }

    private void Idle()
    {
        SetAnimation("Idle");
    }


#if UNITY_EDITOR
    //테스트용
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            HpChanged(5);
            Debug.Log("Test" + hp);
        }
    }
#endif


    #region 이전버전 Raycast
    //private IEnumerator AttackRangeCheck()
    //{
    //    //살아있을때만 raycast 체크
    //    while (!isDead)
    //    {
    //        inAttackRange = CheckRaycast();
    //        yield return null;
    //    }
    //}

    //[SerializeField] private bool m_bDebugMode = false;
    //private RaycastHit hit;
    //private bool CheckRaycast()
    //{
    //    bool inRange = false;
    //    LayerMask layer = LayerMask.GetMask("Monster");

    //    startingPos = transform.position + transform.TransformDirection(Vector3.up) * originOffset;

    //    if (Physics.Raycast(startingPos , transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layer))
    //    {
    //        inRange = true;
    //        Debug.Log(hit.collider.gameObject.name);
    //    }
    //    return inRange;
    //}

    ////Draw Test
    //private void OnDrawGizmos()
    //{
    //    if(m_bDebugMode)
    //    {
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawRay(startingPos, transform.TransformDirection(Vector3.forward) * hit.distance);
    //    }
    //}
    #endregion
}
