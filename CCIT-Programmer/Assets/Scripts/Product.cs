using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Product : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private bool isGet = false;
    public bool IsGet
    {
        get { return isGet; }
        set
        {
            isGet = value;
            PickUp();
        }
    }

    private void Awake()
    {
        StartCoroutine(Rotate());
    }
    private IEnumerator Rotate()
    {
        while (!isGet)
        {
            transform.Rotate(0, 90 * Time.deltaTime * speed, 0);
            yield return null;
        }
    }
    private void PickUp()
    {
        isGet = true;

        //TODO:
        //Particle


    }


}