using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

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
    bool heldObjectColliderEnabled;
    bool heldObjectIsKinematic;

    //variables linked to movement
    [HideInInspector]
    public Vector3 originP;
    public float speed = 3.0f;
    private Vector3 destinationP;
    private bool hasDestination = false;

    //variables linked to rotation
    [HideInInspector]
    public Vector3 originR;
    private bool isRotating = false;
    private Vector3 vecToDestination;
    private float angleToDestination;
    [HideInInspector]public bool canGrab = true;
    private float timer = 0;
    private float grabDelay = 1;
    void Start()
    {
        originP = transform.position;
        originR = transform.eulerAngles;

        //place_holder
        HandPosition = transform;
    }
    
    // Update is called once per frame
    void Update()
    {
        Debug.Log(isHoldingItem);

        //Debug.Log(keyitems[0].transform.position);

        if (!canGrab)
        {
            //Debug.Log("Wagabou");
            timer += Time.deltaTime;
            if (timer >= grabDelay)
            {
                canGrab = true;
                timer = 0;
            }
            return;
        }


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
        transform.Rotate(Vector3.up, angleToDestination/60);
        if (Vector3.Angle(vecToDestination, transform.forward) <= 5.0)
        {
            transform.LookAt(destinationP);
            isRotating = false;
        }
    }

    void GoToDestination()
    {
        transform.position = Vector3.MoveTowards(transform.position, destinationP, speed);
        
        if (Vector3.Distance(transform.position, destinationP) <= 1.5f)
        {
            if (itemFindID != -1)
            {
                if (Vector3.Distance(transform.position, keyitems[itemFindID].transform.position) <= 1.5)
                {
                    HoldObject(keyitems[itemFindID].gameObject, new Vector3(0,0,0));
                }
                destinationP = originP;
                SetRotation();
                itemFindID = -1;
            }
            else
            {
                Debug.Log("has arrived");
                transform.position = destinationP;
                hasDestination = false;
                transform.eulerAngles = originR;
            }
        }
    }

    void SetRotation()
    {
        vecToDestination = destinationP - transform.position;
        angleToDestination = Vector3.SignedAngle(transform.forward, vecToDestination, Vector3.up);
        isRotating = true;
    }
    
    public void HoldObject(GameObject obj, Vector3 rotationOffset)
    {
        Collider heldObjectCollider = obj.GetComponent<Collider>() ? obj.GetComponent<Collider>() : null;
        Rigidbody heldObjectRigidbody = obj.GetComponent<Rigidbody>() ? obj.GetComponent<Rigidbody>() : null;

        if (heldObjectCollider)
        {
            heldObjectCollider.enabled = false;
        }

        if (heldObjectRigidbody)
        {
            heldObjectRigidbody.isKinematic = true;
        }

        obj.transform.SetParent(HandPosition.transform, true);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.eulerAngles = transform.eulerAngles + rotationOffset;

        isHoldingItem = true;
        pickedObject = keyitems[itemFindID];
    }

    public void DestroyHeldItem()
    {
        if (isHoldingItem)
        {
            Collider heldObjectCollider = pickedObject.GetComponent<Collider>();
            Rigidbody heldObjectRigidbody = pickedObject.GetComponent<Rigidbody>();

            if (heldObjectCollider)
                heldObjectCollider.enabled = heldObjectColliderEnabled;

            if (heldObjectRigidbody)
                heldObjectRigidbody.isKinematic = heldObjectIsKinematic;

            pickedObject.transform.SetParent(null, true);
            pickedObject.transform.position = new Vector3(-1000, -1000, -1000);
            pickedObject = null;
            isHoldingItem = false;
        }

        hasDestination = false;
    }
    
    public void ThrowObject(float distance, Vector3 force)
    {
        if (pickedObject.gameObject != null)
        {
            Collider heldObjectCollider = pickedObject.GetComponent<Collider>();
            Rigidbody heldObjectRigidbody = pickedObject.GetComponent<Rigidbody>();

            if (heldObjectCollider)
                heldObjectCollider.enabled = true;
            
            heldObjectRigidbody.isKinematic = true;

            pickedObject.transform.SetParent(null, true);
            pickedObject.transform.position = transform.position + transform.up * 2;
            heldObjectRigidbody.velocity = new Vector3(100, 15, 100);
            heldObjectRigidbody.AddForce(new Vector3(100, 100, 100), ForceMode.Impulse);
            pickedObject = null;
            canGrab = false;
        }
    }

}
