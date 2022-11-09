using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NPC_GrabItem : MonoBehaviour
{
    //variables linked to holdable items
    public PickableObject[] keyitems;
    [HideInInspector]
    public PickableObject pickedObject;
    public float GuardDistance;
    public Transform HandPosition;
    private int itemFindID = -1;
    [HideInInspector]
    public bool isHoldingItem = false;

    //variables linked to movement
    [HideInInspector]
    public Vector3 originP;
    public float speed = 3.0f;
    private Vector3 destinationP;
    private bool hasDestination = false;

    //variables linked to rotation
    [HideInInspector]
    public Quaternion originR;
    private bool isRotating = false;
    private Vector3 vecToDestination;
    private float angleToDestination;
    void Start()
    {
        originP = transform.position;
        originR = transform.rotation;

        //place_holder
        HandPosition = transform;
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if (isRotating)
            Rotate();
        
        else if (hasDestination)
            GoToDestination();

        if (itemFindID == -1)
        {
            for (int i = 0; i < keyitems.Length; i++)
            {
                if (Vector3.Distance(originP, keyitems[i].transform.position) <= GuardDistance)
                {
                    if (!keyitems[i].isHeld && !isHoldingItem)
                    {
                        destinationP = keyitems[i].transform.position;
                        hasDestination = true;
                        SetRotation();
                        itemFindID = i;
                        i = keyitems.Length;
                    }
                }
            }
        }
        if (itemFindID != -1)
        {
            if (keyitems[itemFindID].isHeld)
            {
                destinationP = originP;
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
            transform.LookAt(destinationP);
            isRotating = false;
        }
    }

    void GoToDestination()
    {
        transform.position = Vector3.MoveTowards(transform.position, destinationP, speed);
        
        if (Vector3.Distance(transform.position, destinationP) <= 1.0f)
        {
            if (itemFindID != -1)
            {
                if (Vector3.Distance(transform.position, keyitems[itemFindID].transform.position) <= 2)
                {
                    keyitems[itemFindID].Grab(HandPosition);
                    isHoldingItem = true;
                    pickedObject = keyitems[itemFindID];
                }
                destinationP = originP;
                SetRotation();
                itemFindID = -1;
            }
            else
            {
                transform.position = destinationP;
                hasDestination = false;
                angleToDestination = Vector3.SignedAngle(transform.forward, originR.eulerAngles, Vector3.up);
                isRotating = true;
            }
        }
    }

    void SetRotation()
    {
        vecToDestination = destinationP - transform.position;
        angleToDestination = Vector3.SignedAngle(transform.forward, vecToDestination, Vector3.up);
        isRotating = true;
    }
}
