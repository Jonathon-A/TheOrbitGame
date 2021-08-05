using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
   public LineRenderer IntitialLR;
   private LineRenderer LR;


    // Start is called before the first frame update
    void Start()
    {
        // LR = gameObject.AddComponent(typeof(LineRenderer)) as LineRenderer;


        

        LR = Instantiate(IntitialLR, gameObject.transform.position, Quaternion.identity);
        LR.startColor = gameObject.GetComponent<SpriteRenderer>().color;
        LR.endColor = gameObject.GetComponent<SpriteRenderer>().color;
        LR.positionCount = 0;


    }
    int i = 0;
    // Update is called once per frame
    void FixedUpdate()
    {

        
        LR.positionCount = i + 1;
        LR.SetPosition(i, gameObject.transform.position);
        i++;
    }
}
