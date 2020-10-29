using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public virtual float HP {get; set;}
    public int lineNumber;
    public bool isDead;
    public virtual void HpChanged(float _damage) { }

}