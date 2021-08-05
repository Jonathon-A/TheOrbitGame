using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyCollisionDetector : MonoBehaviour
{
    public ParticleSystem PS;
    public int BodyHealth;
   
    void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Body") && !Invincible)
        {
            if (gameObject.GetComponent<MassCalculator>())
            {

                var PSSize = PS.main;
                PSSize.startSize = gameObject.GetComponent<MassCalculator>().Radius;

            }
            else {
                var PSSize = PS.main;
                PSSize.startSize = 4;
            }

            
       

            if (gameObject.tag == "Weapon")
            {
                collision.gameObject.GetComponent<BodyCollisionDetector>().DamageBody(1);
            }

            ParticleSystem ps = Instantiate(PS, gameObject.transform.position, Quaternion.identity);
            BodiesController.RemoveBody(gameObject);
            Destroy(gameObject);
            Destroy(ps.gameObject, 1f);
            
        }

        


    }
    public bool Invincible;
    public void DamageBody(int Damage) {
        BodyHealth = BodyHealth - Damage;
        if (BodyHealth <= 0 && !Invincible)
        {
            var PSSize = PS.main;
            PSSize.startSize = gameObject.GetComponent<MassCalculator>().Radius;
            ParticleSystem ps = Instantiate(PS, gameObject.transform.position, Quaternion.identity);
            BodiesController.RemoveBody(gameObject);
            Destroy(gameObject);
            Destroy(ps.gameObject, 1f);

        }
    }


}
