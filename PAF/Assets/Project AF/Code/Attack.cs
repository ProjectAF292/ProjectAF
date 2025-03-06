using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float damage;
    public int per;

    public void Inti(float damage, int per)
    {
        this.damage = damage;
        this.per = per;
    }
}
