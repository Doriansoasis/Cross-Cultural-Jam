using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupDrop : MonoBehaviour
{
    [SerializeField] private Transform MouthTransform;
    [SerializeField] private LayerMask pickUpLayerMask;

    private PickableObject GrabbedObject = null;
    // Update is called once per frame
    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (GrabbedObject == null)
            {
                float pickUpDistance = 2f;
                Physics.Raycast(MouthTransform.position, MouthTransform.forward, out RaycastHit hit, pickUpDistance,
                    pickUpLayerMask);
                Debug.Log(hit.transform);
                if (hit.transform.TryGetComponent(out GrabbedObject))
                {
                    GrabbedObject.Grab(MouthTransform);
                }
            }
            else GrabbedObject.Drop();
        }
    }
}
