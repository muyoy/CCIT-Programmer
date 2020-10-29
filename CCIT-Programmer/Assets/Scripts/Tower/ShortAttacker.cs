// ==============================================================
// 원거리 공격형 타워
// 
//
//  AUTHOR: Yang Se Eun
// CREATED: 2020-10-21
// UPDATED: 2020-10-21
//===============================================================


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortAttacker : Attacker
{
    #region parameter

    #endregion


    protected override void Awake()
    {
        base.Awake();
        //고정
        atkRange = 1;
    }


    protected override void Attack()
    {
        SetAnimation("Attack");

    }


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
