using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NPC_GrabItem : MonoBehaviour
{
    //variables linked to holdable items
    public GameObject[] keyitems;
    public float GuardDistance;
    public Transform HandPosition;
    private int itemFindID = -1;

    //variables linked to movement
    private Transform origin;
    public float speed = 3.0f;
    private Transform destination = null;

    //variables linked to rotation
    private bool isRotating = false;
    private Vector3 vecToDestination;
    private float angleToDestination;
    
    void Start()
    {
        origin = transform;
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if (isRotating)
            Rotate();
        
        else if (destination != null)
            GoToDestination();
            
        if (itemFindID == -1)
        {
            for (int i = 0; i < keyitems.Length; i++)
            {
                if (Vector3.Distance(origin.position, keyitems[i].transform.position) <= GuardDistance)
                {
                    if (!keyitems[i].GetComponent<PickableObject>().isHeld)
                    {
                        destination = keyitems[i].transform;
                        SetRotation();
                        itemFindID = i;
                    }
                }
            }
        }
        if (itemFindID != -1)
        {
            if (keyitems[itemFindID].GetComponent<PickableObject>().isHeld)
            {
                destination = origin;
                SetRotation();
                itemFindID = -1;
            }
        }
    }

    void Rotate()
    {
        transform.Rotate(Vector3.up, angleToDestination/30);
        if (Vector3.Angle(vecToDestination, transform.forward) <= 5.0)
        {
            transform.LookAt(destination.transform.position);
            isRotating = false;
        }
    }

    void GoToDestination()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination.position, speed);
        
        if (Vector3.Distance(transform.position, destination.position) <= 1.0f)
        {
            if (itemFindID != -1)
            {
                keyitems[itemFindID].GetComponent<PickableObject>().Grab(HandPosition);
                destination = origin;
                SetRotation();
            }
            else
            {
                transform.position = destination.position;
                destination = null;
                angleToDestination = Vector3.SignedAngle(transform.forward, origin.forward, Vector3.up);
                isRotating = true;
            }
        }
    }

    void SetRotation()
    {
        vecToDestination = destination.position - transform.position;
        angleToDestination = Vector3.SignedAngle(transform.forward, vecToDestination, Vector3.up);
        isRotating = true;
    }
}
