using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Predictor : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {


        Predict();
    }
    public void Predict()
    {




        //LineRenderer[] LRs = new LineRenderer[FakeBodies.Count];
        //for (int i = 0; i < LRs.Length; i++)
        //{
        //    LRs[i] = Instantiate(IntitialLR, FakeBodies[i].transform.position, Quaternion.identity);
        //    LRs[i].startColor = FakeBodies[i].GetComponent<SpriteRenderer>().color;
        //    LRs[i].endColor = FakeBodies[i].GetComponent<SpriteRenderer>().color;
        //    LRs[i].positionCount = 0;
        //}

        //for (int i = 0; i < 100; i++)
        //{
        //    for (int j = 0; j < LRs.Length; j++)
        //    {



        //        predictionPhysicsScene.Simulate(Time.fixedDeltaTime);
        //        LRs[j].positionCount = n + 1;
        //        LRs[j].SetPosition(n, FakeBodies[j].transform.position);


        //    }
        //    n++;
        //}
        //DestroyAllFakeBodies();
    }
}
