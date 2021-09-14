using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodiesController : MonoBehaviour
{
    private static List<BodyProperties> AllBodies = new List<BodyProperties>();


    public GameObject Subject;
    public int MaxSteps;
    private static int ActualMaxSteps;


    private static GravitationalForce ActualSubject;

    // Start is called before the first frame update
    void Start()
    {

        ActualMaxSteps = MaxSteps;
        try
        {
            ActualSubject = Subject.GetComponent<GravitationalForce>();
        }
        catch (System.Exception)
        {
            ActualSubject = null;
        }

        InitialiseArrays(false);
        InitialiseAllFutureTrajectories(ActualMaxSteps, ActualSubject);

    }

    public static void InitialiseArrays(bool LinesPrexist)
    {
        AllBodies.Clear();

        GameObject[] AllBodiesObjects = GameObject.FindGameObjectsWithTag("Body");

        for (int i = 0; i < AllBodiesObjects.Length; i++)
        {
            AllBodies.Add(new BodyProperties(AllBodiesObjects[i], AllBodiesObjects[i].GetComponent<Rigidbody2D>(),
                AllBodiesObjects[i].GetComponent<GravitationalForce>()));

            AllBodies[i].GetBodyScript().InitialiseBody(AllBodies[i].GetBodyScript().InitialVelocity);

            if (!LinesPrexist)
            {

                AllBodies[i].GetBodyScript().InitialiseLine();
                AllBodies[i].GetBodyScript().InitialisePredictionBody();


            }

        }


    }

    public static void RecalculateAllFutureTrajectories(int Steps, GravitationalForce Subject)
    {

        foreach (BodyProperties Body in AllBodies)
        {
            Body.GetBodyScript().ResetTrajectoryQueue();
        }

        InitialiseAllFutureTrajectories(Steps, Subject);
    }


    public static void InitialiseAllFutureTrajectories(int Steps, GravitationalForce Subject)
    {
        for (int i = 0; i < Steps; i++)
        {
            foreach (BodyProperties Body in AllBodies)
            {

                Body.GetBodyScript().CalculateFutureVelocity();
            }
            foreach (BodyProperties Body in AllBodies)
            {

                Body.GetBodyScript().CalculateFuturePosition(Subject);
                Body.GetBodyScript().SetCollisionPositions(-1);


            }

            Physics2D.SyncTransforms();
            foreach (BodyProperties Body in AllBodies)
            {

                Body.GetBodyScript().CheckForCollisions();

            }
            foreach (BodyProperties Body in AllBodies)
            {

                Body.GetBodyScript().MoveIfCollided();

            }


        }

    }
    public static void InitialiseFutureTrajectory(int Steps, GravitationalForce Subject, GravitationalForce NewBody)
    {
        for (int i = 0; i < Steps; i++)
        {
            List<NewTrajectoryProperies> PreviousBodiesTrajectoryProperties = new List<NewTrajectoryProperies>();
            foreach (BodyProperties Body in AllBodies)
            {
                if (!Body.GetBodyObject().CompareTag("Weapon") && !Body.GetBodyScript().IsCollidedAtIndex(i))
                {
                    PreviousBodiesTrajectoryProperties.Add(Body.GetBodyScript().GetTrajectoryPropertiesAtIndex(i));
                }

            }

            NewBody.CalculateNewFutureVelocity(PreviousBodiesTrajectoryProperties, 1);

            NewBody.CalculateNewFuturePosition(Subject, i, 1);

            NewBody.SetCollisionPositions(-1);

            foreach (BodyProperties Body in AllBodies)
            {
                if (Body.GetBodyScript() != NewBody && !Body.GetBodyScript().IsCollidedAtIndex(i))
                {
                    Body.GetBodyScript().SetCollisionPositions(i);
                }


            }
            Physics2D.SyncTransforms();
            NewBody.CheckForCollisions();
            foreach (BodyProperties Body in AllBodies)
            {

                Body.GetBodyScript().CheckForCollisions();

            }
            NewBody.MoveIfCollided();
            foreach (BodyProperties Body in AllBodies)
            {
                Body.GetBodyScript().MoveIfCollided();


            }

            if (NewBody.IsCollided())
            {
                i = Steps;
            }
        }
    }

    public static void InitialiseApproximateFutureTrajectory(int Steps, GravitationalForce Subject, GravitationalForce NewBody , int Multiplier)
    {
        for (int i = 0; i < Steps; i+= Multiplier)
        {
            List<NewTrajectoryProperies> PreviousBodiesTrajectoryProperties = new List<NewTrajectoryProperies>();
            foreach (BodyProperties Body in AllBodies)
            {
                if (!Body.GetBodyObject().CompareTag("Weapon") && !Body.GetBodyScript().IsCollidedAtIndex(i))
                {
                    PreviousBodiesTrajectoryProperties.Add(Body.GetBodyScript().GetTrajectoryPropertiesAtIndex(i));
                }

            }

            NewBody.CalculateNewFutureVelocity(PreviousBodiesTrajectoryProperties, Multiplier);

            NewBody.CalculateNewFuturePosition(Subject, i, Multiplier);

            NewBody.SetCollisionPositions(-1);

            foreach (BodyProperties Body in AllBodies)
            {
                if (Body.GetBodyScript() != NewBody && !Body.GetBodyScript().IsCollidedAtIndex(i))
                {
                    Body.GetBodyScript().SetCollisionPositions(i);
                }


            }
            Physics2D.SyncTransforms();
            NewBody.CheckForCollisions();
            foreach (BodyProperties Body in AllBodies)
            {

                Body.GetBodyScript().CheckForCollisions();

            }
            NewBody.MoveIfCollided();
            foreach (BodyProperties Body in AllBodies)
            {
                Body.GetBodyScript().MoveIfCollided();


            }

            if (NewBody.IsCollided())
            {
                i = Steps;
            }
        }
    }


    private static int StepsCount = 0;
    private bool TrajectoriesToBeCleared = false;
    void FixedUpdate()
    {

        foreach (BodyProperties Body in AllBodies)
        {

            Body.GetBodyScript().CalculateFutureVelocity();


        }
        foreach (BodyProperties Body in AllBodies)
        {

            Body.GetBodyScript().CalculateFuturePosition(ActualSubject);
            Body.GetBodyScript().SetCollisionPositions(-1);

        }
        Physics2D.SyncTransforms();
        foreach (BodyProperties Body in AllBodies)
        {

            Body.GetBodyScript().CheckForCollisions();

        }

        foreach (BodyProperties Body in AllBodies)
        {


            Body.GetBodyScript().MoveIfCollided();
            Body.GetBodyScript().UpdatePosition();

        }

        for (int i = 0; i < AllBodies.Count; i++)
        {
            if (AllBodies[i].GetBodyScript().RemoveIfDestroyed())

            {
                i--;
            }
        }


        if (Input.GetMouseButton(0))// Input.GetMouseButton(0)
        {
            if (StepsCount < ActualMaxSteps && Input.GetKey("w"))
            {
                StepsCount++;
            }
            if (StepsCount > 0 && Input.GetKey("s"))
            {
                StepsCount--;
            }

            //  StepsCount = ActualMaxSteps;
            foreach (BodyProperties Body in AllBodies)
            {

                // Body.GetBodyScript().DrawTrajectory(Mathf.RoundToInt(Mathf.Log(StepsCount + 1, 1.01251082591f)),false);
                Body.GetBodyScript().DrawTrajectory(StepsCount, false);

            }




            // PredictTrajectory(StepsCount);
            TrajectoriesToBeCleared = true;
        }
        else if (TrajectoriesToBeCleared)

        {
            StepsCount = 0;
            foreach (BodyProperties Body in AllBodies)
            {

                Body.GetBodyScript().ResetPrediction();

            }

            TrajectoriesToBeCleared = false;
        }
    }


    public static void AddBody(GameObject Body, bool Aiming)
    {

        // Debug.Log(AllBodies.Count);


        GravitationalForce BodyScript = Body.GetComponent<GravitationalForce>();
        BodyScript.InitialiseBody(BodyScript.InitialVelocity);
        BodyScript.InitialiseLine();
        BodyScript.InitialisePredictionBody();
        if (Body.CompareTag("Weapon"))
        {
            

            if (!Aiming)
            {
                InitialiseFutureTrajectory(ActualMaxSteps, ActualSubject, BodyScript);
                AllBodies.Add(new BodyProperties(Body, Body.GetComponent<Rigidbody2D>(),
                   Body.GetComponent<GravitationalForce>()));
         
            }
            else
            {
                InitialiseApproximateFutureTrajectory(ActualMaxSteps, ActualSubject, BodyScript, 1);
                BodyScript.DrawTrajectory(StepsCount, true);
            }

        }





        // Debug.Log(AllBodies.Count);

    }


    public static void RemoveBody(GameObject Body)
    {

        for (int i = AllBodies.Count - 1; i >= 0; i--)
        {
            if (AllBodies[i].GetBodyObject() == Body)
            {
                AllBodies.RemoveAt(i);
                i = -1;
            }
        }

        //AllBodies.Remove(new BodyProperties(Body, Body.GetComponent<Rigidbody2D>(),
        //          Body.GetComponent<GravitationalForce>()));
        GravitationalForce BodyScript = Body.GetComponent<GravitationalForce>();
        BodyScript.ResetPrediction();
        BodyScript.RemoveLine();
        BodyScript.RemovePredictionBody();

        //if (Body.tag != "Weapon")
        //{
        //    //   RecalculateAllFutureTrajectories(ActualMaxSteps, ActualSubject);

        //}

    }

    public static List<BodyProperties> GetAllBodies()
    {
        return AllBodies;
    }

}
