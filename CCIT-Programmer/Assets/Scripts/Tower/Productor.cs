// ==============================================================
// 생산형 타워
// 
//
//  AUTHOR: Yang Se Eun
// CREATED: 2020-10-21
// UPDATED: 2020-10-21
//===============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Productor : Tower
{
    [SerializeField] protected GameObject productObj = null;                         //생성할 오브젝트
    [SerializeField] protected float createTime = 1.0f;                              //생성하는데 필요한 시간
    public float speed = 5.0f;



    protected override void Awake()
    {
        type = Type.Productor;
    }


    protected override void Work()
    {
        base.Work();

        StartCoroutine(CreateProduct());
    }

    //자원생산
    private IEnumerator CreateProduct()
    {
        while(!isDead)
        {
            //Create milk
            GameObject obj = Instantiate(productObj, startingPos, Quaternion.identity);

            Vector3 v = Random.insideUnitCircle;
            Vector3 direction = new Vector3(v.x, 0.0f,v.y).normalized + Vector3.up;
            obj.GetComponent<Rigidbody>().AddForce(direction * speed, ForceMode.Impulse);

            yield return new WaitForSeconds(createTime);
        }


    }

}
