using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;
    public Transform target;
    Camera Cam;
    void Start()
    {
        Cam = this.GetComponent<Camera>();
    }
    void FixedUpdate()
    {
        Vector3 point = Cam.WorldToViewportPoint(target.position);
        Vector3 delta = target.position - Cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
        Vector3 destination = transform.position + delta;
        transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
    }
}

