using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogBark : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f); //Transparent Red

        if (GetComponent<SphereCollider>())
            Gizmos.DrawSphere(transform.position, GetComponent<SphereCollider>().radius);
    }

    private void OnTriggerEnter(Collider other)
    {
        //This will side a signal to the NPC once we have them
    }
}
