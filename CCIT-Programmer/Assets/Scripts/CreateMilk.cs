using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMilk : MonoBehaviour
{

    public GameObject milk_prefab;
    private Vector3 pos;
    public float power;
    private float cooltime;

    private void Start()
    {
        pos = transform.position; 
    }
    private void Update()
    {
        cooltime += Time.deltaTime;
        if(cooltime > 5.0f)
        {
            cooltime = 0.0f;
            Create(); 
        }
    }

    private void Create()
    {
        GameObject milk = Instantiate(milk_prefab, pos, Quaternion.identity);
        milk.name = "Milk";
        float x = Random.Range(-1.0f, 1.0f);
        float z = Random.Range( 1.0f, 1.5f);
        milk.GetComponent<Rigidbody>().AddForce(new Vector3(x, 1.0f * power, z), ForceMode.Impulse);
        cooltime = 0.0f;
    }
}
