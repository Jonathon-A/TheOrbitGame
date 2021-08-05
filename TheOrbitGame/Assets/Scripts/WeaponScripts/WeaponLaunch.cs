using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLaunch : MonoBehaviour
{
    private GravitationalForce ThisBodyScript;
    private float Radius; 
    private void Start()
    {
         ThisBodyScript = gameObject.GetComponent<GravitationalForce>();
        MassCalculator ThisScript = gameObject.GetComponent<MassCalculator>();
        Radius = ThisScript.Radius;

    }
    private void OnMouseDown()
    {

        WeaponController.SetLaunchBody(gameObject, Radius, ThisBodyScript.GetCurrentVelocity()) ;

    }





    private void OnMouseUp()
    {
        WeaponController.AttempRocketLaunch();
    }
}
