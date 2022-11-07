using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
    public int questID = 0;
    private Rigidbody objectBody;
    public Transform mouthPosition;
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
        }
    }

    public void Grab(Transform objectGrabPoint)
    {
        this.mouthPosition = objectGrabPoint;
        objectBody.useGravity = false;
    }

    public void Drop()
    {
        objectBody.useGravity = true;
        mouthPosition = null;
    }
}
