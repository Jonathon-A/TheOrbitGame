using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MassCalculator : MonoBehaviour
{
    public float Radius;
    public float SurfaceGravitationalFieldStrength;

    void Update() {
        gameObject.transform.localScale = new Vector3(Radius, Radius, 1);
        gameObject.GetComponent<Rigidbody2D>().mass = (SurfaceGravitationalFieldStrength * Mathf.Pow(Radius, 2f)) / 66.7408f;

        //print(gameObject.GetComponent<Rigidbody2D>().mass);
    }
}
