using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
    private Rigidbody objectBody;
    private Transform mouthPosition;
    [HideInInspector]
    public bool isHeld = false;
    // Start is called before the first frame update
    private void Awake()
    {
        objectBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (mouthPosition != null)
        {
            objectBody.MovePosition(mouthPosition.position);
            transform.LookAt(mouthPosition.forward);
        }
    }

    public void Grab(Transform objectGrabPoint)
    {
        if (isHeld)
            return;
        this.mouthPosition = objectGrabPoint;
        objectBody.useGravity = false;
        isHeld = true;
    }

    public void Drop()
    {
        objectBody.useGravity = true;
        mouthPosition = null;
        isHeld = false;
    }
}
