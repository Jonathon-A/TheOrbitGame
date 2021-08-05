using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyProperties
{
    private GameObject BodyObject;
    private Rigidbody2D BodyPhysics;
    private GravitationalForce BodyScript;
    public BodyProperties(GameObject NewBodyObject,    Rigidbody2D NewBodyPhysics,    GravitationalForce NewBodyScript)
    {
        this.BodyObject = NewBodyObject;
        this.BodyPhysics = NewBodyPhysics;
        this.BodyScript = NewBodyScript;
     
    }

    public GameObject GetBodyObject() {
        return BodyObject;
    }

    public Rigidbody2D GetBodyPhysics()
    {
        return BodyPhysics;
    }

    public GravitationalForce GetBodyScript()
    {
        return BodyScript;
    }

  



}
