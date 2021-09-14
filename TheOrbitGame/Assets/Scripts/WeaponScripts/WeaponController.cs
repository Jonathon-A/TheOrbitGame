using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public static float LaunchOffset = 5.2f;
    void Start()
    {
        Rocket = InitialRocket;


        LR2 = LR;
    }

    private bool RocketReady = true;
    //private void FixedUpdate()
    //{
    //    if (Input.GetKey("space") && RocketReady)
    //    {

    //        if (LaunchBody)
    //        {

    //            RocketReady = false;
    //            LaunchRocket(new Vector3(100,100,0), new Vector2(Random.Range(-20.0f, 20.0f), Random.Range(-20.0f, 20.0f)));


    //            Invoke("ReadyRocket", 1);

    //        }



    //    }
    //}
    private static Vector2 LaunchVector = Vector2.zero;
    private static Vector3 Position;
    private static Vector2 Velocity;
    
    public float Sensitivity;

    private Vector3 DragOrigin;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DragOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0) && LaunchBody)
        {
            Vector3 Difference = (DragOrigin - Camera.main.ScreenToWorldPoint(Input.mousePosition)) * Sensitivity;
            // print("origin" + DragOrigin + " newPosition " + Cam.ScreenToWorldPoint(Input.mousePosition) + " = difference " + Difference);

          
          

            Vector3 LaunchVector3 = -Difference;

            LaunchVector = new Vector2(LaunchVector3.x, LaunchVector3.y) + BodyVelocity;
            //Debug.Log(LaunchBody.name + " " + LaunchVector.magnitude);

            Position = new Vector3(LaunchBody.transform.position.x, LaunchBody.transform.position.y, 0) + new Vector3(LaunchVector.x, LaunchVector.y, 0).normalized * (Radius + LaunchOffset);
            Velocity = (LaunchVector - LaunchVector.normalized * (Radius + LaunchOffset));// + BodyVelocity;

            if (LaunchVector.magnitude > Radius && LaunchBody)
            {

                PreviewLaunchRocket(Position, Velocity);
                LR.positionCount = 2;
                LR.SetPosition(0, new Vector3(LaunchBody.transform.position.x, LaunchBody.transform.position.y, 4));
                LR.SetPosition(1, new Vector3((LaunchBody.transform.position - Difference).x , (LaunchBody.transform.position - Difference).y , 4));


            }


            //  LR2.positionCount = 2;
            // LR2.SetPosition(0, LaunchBody.transform.position);
            //  LR2.SetPosition(1, Position);
        }
        else {
            OldLaunchedAimRocket = LaunchedAimRocket;
            LaunchedAimRocket = null;
            LR.positionCount = 0;
           
        
        }
        if (OldLaunchedAimRocket)
        {

            BodiesController.RemoveBody(OldLaunchedAimRocket);
            Destroy(OldLaunchedAimRocket);
            OldLaunchedAimRocket = null;
        }
      

    }

    private void ReadyRocket()
    {
        RocketReady = true;
        // print("ready");
    }

    private static GameObject LaunchBody;
    private static float Radius;
    private static Vector2 BodyVelocity;
    public static void SetLaunchBody(GameObject NewLaunchBody, float NewRadius, Vector2 NewBodyVelocity)
    {
        LaunchBody = NewLaunchBody;
        Radius = NewRadius / 2;
        BodyVelocity = NewBodyVelocity;
    

    }
    public LineRenderer LR;
    public static LineRenderer LR2;

    public static void AttempRocketLaunch()
    {
        if (LaunchVector.magnitude > Radius && LaunchBody)
        {

            LaunchRocket(Position, Velocity);

        }

        LaunchBody = null;
        Radius = -1f;
    }


    public GameObject InitialRocket;
    public static GameObject Rocket;

    public static void LaunchRocket(Vector3 InitialPosition, Vector2 InitialVelocity)
    {

        float angle = Mathf.Atan2(InitialVelocity.y, InitialVelocity.x) * Mathf.Rad2Deg;

        GameObject LaunchedRocket = Instantiate(Rocket, InitialPosition, Quaternion.Euler(new Vector3(0, 0, angle)));
        LaunchedRocket.GetComponent<GravitationalForce>().InitialVelocity = InitialVelocity;

        BodiesController.AddBody(LaunchedRocket, false);

    }
    private static GameObject OldLaunchedAimRocket;
    private static GameObject LaunchedAimRocket;
    public static void PreviewLaunchRocket(Vector3 InitialPosition, Vector2 InitialVelocity)
    {
        OldLaunchedAimRocket = LaunchedAimRocket;

        float angle = Mathf.Atan2(InitialVelocity.y, InitialVelocity.x) * Mathf.Rad2Deg;

        LaunchedAimRocket = Instantiate(Rocket, InitialPosition, Quaternion.Euler(new Vector3(0, 0, angle)));
        LaunchedAimRocket.GetComponent<GravitationalForce>().InitialVelocity = InitialVelocity;
        LaunchedAimRocket.GetComponent<SpriteRenderer>().enabled = false;

       // BodiesController.AddBody(LaunchedAimRocket, true);
        BodiesController.AddBody(LaunchedAimRocket, true);

    }
}
