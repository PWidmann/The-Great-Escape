using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private float targetCameraZoom;
    private float cameraZoomRate = 3f;
    private float cameraSmoothing = 0.05f;
    private float currentCameraZoom;
    
    void Start()
    {
        targetCameraZoom = Camera.main.orthographicSize;
    }

    private void LateUpdate()
    {
        //Camera Zoom
        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            targetCameraZoom += Input.GetAxis("Mouse ScrollWheel") * cameraZoomRate * -1;
            //currentCameraSize += Input.GetAxis("Mouse ScrollWheel") * cameraZoomRate * -1;
        }
        currentCameraZoom = Camera.main.orthographicSize;

        Camera.main.orthographicSize = Mathf.Lerp(currentCameraZoom, targetCameraZoom, cameraSmoothing);
    }

    
    void Update()
    {
        
    }
}
