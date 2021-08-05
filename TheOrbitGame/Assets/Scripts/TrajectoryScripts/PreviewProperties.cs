using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewProperties : MonoBehaviour
{
    private int Damage;
   // private bool CollisionDetected;

    public void SetDamage(int NewDamage)
    {
        Damage = NewDamage;
    }

    public int GetDamage()
    {
        return Damage;
    }


    //public void SetCollisionDetected(bool NewCollisionDetected)
    //{
    //    CollisionDetected = NewCollisionDetected;
    //}

    //public bool IsCollisionDetected()
    //{
    //    return CollisionDetected;
    //}
}
