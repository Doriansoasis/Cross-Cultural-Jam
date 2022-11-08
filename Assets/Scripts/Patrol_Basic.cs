using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.UIElements;
using Vector3 = UnityEngine.Vector3;

public class Patrol_Basic: MonoBehaviour
{
    //variables linked to movement
    public Transform[] waypoints;
    private int index = 0;
    private bool canPatrol = true;
    public bool loop = true;
    private float distanceFromPoint;
    public float speed = 1;
    
    //variables linked to rotation
    private bool rotating = false;
    private Vector3 vecToWaypoint;
    private float angleToPoint;
    
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
            Debug.Log(distanceFromPoint);
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
                angleToPoint = Vector3.SignedAngle(transform.forward, vecToWaypoint, Vector3.up);
                rotating = true;
            }
            else Patrol();
        }
    }

    void Rotate()
    {
        transform.Rotate(Vector3.up, angleToPoint/30);
        if (Vector3.Angle(vecToWaypoint, transform.forward) <= 5.0)
        {
            transform.LookAt(waypoints[index].position);
            rotating = false;
        }
    }

    void Patrol()
    {
        if (canPatrol)
            transform.position = Vector3.MoveTowards(transform.position, waypoints[index].position, speed);
    }
}
