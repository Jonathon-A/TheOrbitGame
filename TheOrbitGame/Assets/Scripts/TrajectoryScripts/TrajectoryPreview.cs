using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class TrajectoryPreview : MonoBehaviour
{

    public bool ShowPreviewTrajectory;

    // Update is called once per frame
    void Update()
    {
        if (!Application.isPlaying)
        {
            PreviewTrajectory();
        }
    }

    private bool FreshRun = true;
    private void OnApplicationQuit()
    {
        FreshRun = true;
    }



    public int StepCount;

    void PreviewTrajectory()
    {

        if (ShowPreviewTrajectory)
        {





            if (GameObject.FindGameObjectsWithTag("PredictionLine").Length != BodiesController.GetAllBodies().Count || FreshRun || GameObject.FindGameObjectsWithTag("PredictionBody").Length != BodiesController.GetAllBodies().Count)
            {
                foreach (GameObject PredictionLine in GameObject.FindGameObjectsWithTag("PredictionLine"))
                {
                    DestroyImmediate(PredictionLine);
                }

                foreach (GameObject PredictionBody in GameObject.FindGameObjectsWithTag("PredictionBody"))
                {
                    DestroyImmediate(PredictionBody);
                }

                BodiesController.InitialiseArrays(false);

                FreshRun = false;
            }
            else
            {
                BodiesController.InitialiseArrays(true);
            }



            if (StepCount > 100000)
            {
                StepCount = 100000;

            }
            if (StepCount < 0)
            {
                StepCount = 0;
            }
            if (StepCount > 0)
            {
                PredictTrajectory(StepCount);
            }
            else
            {
                foreach (GameObject PredictionLine in GameObject.FindGameObjectsWithTag("PredictionLine"))
                {
                    DestroyImmediate(PredictionLine);
                }
                foreach (GameObject PredictionBody in GameObject.FindGameObjectsWithTag("PredictionBody"))
                {
                    DestroyImmediate(PredictionBody);
                }
                FreshRun = true;
            }


        }
        else
        {
            foreach (GameObject PredictionLine in GameObject.FindGameObjectsWithTag("PredictionLine"))
            {
                DestroyImmediate(PredictionLine);
            }
            foreach (GameObject PredictionBody in GameObject.FindGameObjectsWithTag("PredictionBody"))
            {
                DestroyImmediate(PredictionBody);
            }
            FreshRun = true;
        }

    }


    public GameObject Subject;

    private void PredictTrajectory(int Steps)
    {



        List<BodyProperties> AllBodies = BodiesController.GetAllBodies();
        for (int i = 0; i < AllBodies.Count; i++)
        {


            AllBodies[i].GetBodyScript().ResetPrediction();
            AllBodies[i].GetBodyScript().ResetPreview();



        }



        GravitationalForce ActualSubject;
        try
        {
            ActualSubject = Subject.GetComponent<GravitationalForce>();
        }
        catch (System.Exception)
        {
            ActualSubject = null;
        }

        BodiesController.InitialiseAllFutureTrajectories(Steps, ActualSubject);

        foreach (BodyProperties Body in AllBodies)
        {
            Body.GetBodyScript().DrawTrajectory(Steps, true);
        }

      

    }
}
