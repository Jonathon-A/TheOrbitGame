using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Camera Cam;

    public float MinSize = 20f;
    public float MaxSize = 200f;
    public float ZoomStep = 10f;

    private float MinXBound, MaxXBound, MinYBound, MaxYBound;

    private void Awake()
    {
        Cam = gameObject.GetComponent<Camera>();

        MinXBound = Cam.transform.position.x - (Cam.orthographicSize * Cam.aspect);
        MaxXBound = Cam.transform.position.x + (Cam.orthographicSize * Cam.aspect);

        MinYBound = Cam.transform.position.x - Cam.orthographicSize;
        MaxYBound = Cam.transform.position.x + Cam.orthographicSize;

    }


    // Update is called once per frame
    private void Update()
    {
       // var View = Cam.ScreenToViewportPoint(Input.mousePosition);
       // var isOutside = View.x < 0 || View.x > 1 || View.y < 0 || View.y > 1;
       //if (!isOutside)
       // {
            PanCamera();
        // }

        //if (Input.GetAxis("Mouse ScrollWheel") > 0)
        //{
        //    ZoomIn();

        //}

        //if (Input.GetAxis("Mouse ScrollWheel") < 0)
        //{
        //    ZoomOut();
        //}

        // Scroll forward
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            ZoomOrthoCamera(Camera.main.ScreenToWorldPoint(Input.mousePosition), 1);
        }

        // Scoll back
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            ZoomOrthoCamera(Camera.main.ScreenToWorldPoint(Input.mousePosition), -1);
        }

    }

    private Vector3 DragOrigin;

    private void PanCamera() {
        if (Input.GetMouseButtonDown(2))
        {
            DragOrigin = Cam.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(2))
        {
            Vector3 Difference = DragOrigin - Cam.ScreenToWorldPoint(Input.mousePosition);
            // print("origin" + DragOrigin + " newPosition " + Cam.ScreenToWorldPoint(Input.mousePosition) + " = difference " + Difference);

            Cam.transform.position = ClampCamera(Cam.transform.position + Difference);
           
        }

    }

    private void ZoomOrthoCamera(Vector3 zoomTowards, float Direction)
    {

        // Calculate how much we will have to move towards the zoomTowards position
        float multiplier = (1.0f / Cam.orthographicSize * Direction);

        Vector3 Translation = (zoomTowards - transform.position) * multiplier;
        Translation.z = 0;
        // Move camera
        //Debug.Log(Translation);
        transform.position += Translation * ZoomStep;

        // Zoom camera
        Cam.orthographicSize -= Direction * ZoomStep;

        // Limit zoom
        Cam.orthographicSize = Mathf.Clamp(Cam.orthographicSize, MinSize, MaxSize);

        Cam.transform.position = ClampCamera(Cam.transform.position);
    }

    //private void ZoomIn() {
    //    float NewSize = Cam.orthographicSize - ZoomStep;
    //    Cam.orthographicSize = Mathf.Clamp(NewSize, MinSize, MaxSize);

    //    Cam.transform.position = ClampCamera(Cam.transform.position);
    //}

    //private void ZoomOut() {
    //    float NewSize = Cam.orthographicSize + ZoomStep;
    //    Cam.orthographicSize = Mathf.Clamp(NewSize, MinSize, MaxSize);

    //    Cam.transform.position = ClampCamera(Cam.transform.position);
    //}

    private Vector3 ClampCamera(Vector3 TargetPosition) {

        float CamHeight = Cam.orthographicSize;
        float CamWidth = Cam.orthographicSize * Cam.aspect;

        float MinX = MinXBound + CamWidth;
        float MaxX = MaxXBound - CamWidth;
        float MinY = MinYBound + CamHeight;
        float MaxY = MaxYBound - CamHeight;

        float NewX = Mathf.Clamp(TargetPosition.x, MinX, MaxX);
        float NewY = Mathf.Clamp(TargetPosition.y, MinY, MaxY);

        return new Vector3(NewX, NewY, TargetPosition.z);
    }
}
