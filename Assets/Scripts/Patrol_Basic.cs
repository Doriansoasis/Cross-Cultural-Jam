using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.UIElements;
using Vector3 = UnityEngine.Vector3;

public class Patrol_Basic: MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 1;
    private int index = 0;
    private bool rotating = false;
    public bool loop = true;
    private float distanceFromPoint;
    private bool canPatrol = true;

    private Vector3 vecToWaypoint;

    private float angleToPoint = 0;
    void Start()
    {
        transform.LookAt(waypoints[0].position);
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rotating)
            Rotate();
        else
        {
            distanceFromPoint = Vector3.Distance(waypoints[index].position,transform.position);
            if (distanceFromPoint <= 0.5)
            {
                index++;
                if (index >= waypoints.Length)
                {
                    index = 0;
                    if(!loop) 
                        canPatrol = false;
                }

                vecToWaypoint = waypoints[index].position - transform.position;
                angleToPoint = Vector3.SignedAngle(vecToWaypoint, transform.forward, Vector3.up);
                rotating = true;
            }
            else Patrol();
        }
    }

    void Rotate()
    {
        
        transform.Rotate(Vector3.up, -angleToPoint/30);
        if (Mathf.Abs(Vector3.Angle(vecToWaypoint, transform.forward)) <= 5.0)
        {
            transform.LookAt(waypoints[index].position);
            rotating = false;
        }
    }

    void Patrol()
    {
        if (canPatrol)
            transform.Translate(Vector3.forward*speed);
    }
}
