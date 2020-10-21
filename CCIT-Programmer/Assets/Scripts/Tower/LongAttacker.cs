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

public class LongAttacker : Attacker
{
    #region parameter

    [SerializeField] private GameObject projectile = null;
    public float speed = 2.0f;
    #endregion


  
    protected override void Attack()
    {
        SetAnimation("Attack");

        GameObject obj = Instantiate(projectile, startingPos, Quaternion.identity);
        Vector3 direction = transform.TransformDirection(Vector3.forward);
        obj.GetComponent<Rigidbody>().AddForce(direction * speed * 100.0f, ForceMode.Force);

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
