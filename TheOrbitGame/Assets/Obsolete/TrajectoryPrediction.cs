using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrajectoryPrediction : MonoBehaviour
{




    //private List<GameObject> AllBodies = BodiesController.GetAllBodies();
    private List<GameObject> FakeBodies = new List<GameObject>();

    private Scene predictionScene;
    PhysicsScene2D currentPhysicsScene;
    PhysicsScene2D predictionPhysicsScene;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.simulationMode = SimulationMode2D.Script;

        currentPhysicsScene = SceneManager.GetActiveScene().GetPhysicsScene2D();

        CreateSceneParameters parameters = new CreateSceneParameters(LocalPhysicsMode.Physics2D);
        predictionScene = SceneManager.CreateScene("PredictionScene", parameters);
        predictionPhysicsScene = predictionScene.GetPhysicsScene2D();
       // Invoke("CopyAllBodies", 2.0f);

        CopyAllBodies();
      Predict();
    }

    void FixedUpdate()
    {
        if (currentPhysicsScene.IsValid()
            //  && predictionPhysicsScene.IsValid()
            )
        {
          
            currentPhysicsScene.Simulate(Time.fixedDeltaTime);
           // predictionPhysicsScene.Simulate(Time.fixedDeltaTime);
        }
        }


    public void CopyAllBodies()
    {
        //foreach (GameObject Body in AllBodies)
        //{
            
        //    GameObject FakeBody = Instantiate(Body);
            

        //    Renderer FakeBodyRenderer = FakeBody.GetComponent<SpriteRenderer>();
        //    if (FakeBodyRenderer)
        //    {
        //         FakeBodyRenderer.enabled = false;
        //    }

        //    foreach (Transform child in FakeBody.transform)
        //    {
        //        Destroy(child.gameObject);
        //    }
        //   // FakeBody.GetComponent<GravitationalForce>().SetFakeBody();
        //    SceneManager.MoveGameObjectToScene(FakeBody, predictionScene);

        //  //  BodiesController.AddFakeBody(FakeBody);
        //    FakeBody.GetComponent<Rigidbody2D>().velocity = Body.GetComponent<Rigidbody2D>().velocity;
        //    FakeBody.transform.position = Body.transform.position;
        //    FakeBody.transform.rotation = Body.transform.rotation;
            
        //    FakeBodies.Add(FakeBody);

        //  //  FakeBody.GetComponent<Rigidbody2D>().AddForce(new Vector2(150, 100), ForceMode2D.Impulse);
        //   // Body.GetComponent<Rigidbody2D>().AddForce(new Vector2(150, 100), ForceMode2D.Impulse);
        //}
       

    }
    public void DestroyAllFakeBodies()
    {
        foreach (GameObject FakeBody in FakeBodies)
        {
           // BodiesController.RemoveFakeBody(FakeBody);
            Destroy(FakeBody);
        }
        FakeBodies.Clear();
    }

    public LineRenderer IntitialLR;

    private int n = 0;
    public void Predict() {




        LineRenderer[] LRs = new LineRenderer[FakeBodies.Count];
        for (int i = 0; i < LRs.Length; i++)
         {
            LRs[i] = Instantiate(IntitialLR, FakeBodies[i].transform.position, Quaternion.identity);
            LRs[i].startColor = FakeBodies[i].GetComponent<SpriteRenderer>().color;
            LRs[i].endColor = FakeBodies[i].GetComponent<SpriteRenderer>().color;
            LRs[i].positionCount = 0;
        }

        for (int i = 0; i < 1000; i++)
        {
            foreach (GameObject FakeBody in FakeBodies)
            {
                FakeBody.GetComponent<Rigidbody2D>().AddForce(new Vector2(10, 50) * Time.deltaTime, ForceMode2D.Impulse);
            }
            
            predictionPhysicsScene.Simulate(Time.fixedDeltaTime);

            for (int j = 0; j < LRs.Length; j++)
            {

              

                
                LRs[j].positionCount = n + 1;
                LRs[j].SetPosition(n, FakeBodies[j].transform.position);

                
            }
            n++;
        }
        DestroyAllFakeBodies();
    }

    

    }





