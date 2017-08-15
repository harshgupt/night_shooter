using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour {

    //The target for the camera to follow
    public Transform target;
    //Smooth movement of camera
    public float smoothing = 5.0f;

    //Distance of camera from Player
    Vector3 offset;

    void Start()
    {
        offset = transform.position - target.position;
    }

    void FixedUpdate()
    {
        //The target position that the camera tries to reach
        Vector3 targetCameraPosition = target.position + offset;
        //Lerp is for Linear Interpolation for the smooth translation to new position, in fixed time (smoothing)
        transform.position = Vector3.Lerp(transform.position, targetCameraPosition, smoothing * Time.deltaTime);
    }

}
