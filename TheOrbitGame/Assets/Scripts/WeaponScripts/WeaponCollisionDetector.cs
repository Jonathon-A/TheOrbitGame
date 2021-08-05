using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollisionDetector : MonoBehaviour
{
    public ParticleSystem PS;

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Weapon"))
        {
            var PSSize = PS.main;
            PSSize.startSize = 4;


            ParticleSystem ps = Instantiate(PS, gameObject.transform.position, Quaternion.identity);
            BodiesController.RemoveBody(gameObject);
            Destroy(gameObject);
            Destroy(ps.gameObject, 1f);




        }


    }
}
