using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatJumpSpot : MonoBehaviour
{
    [HideInInspector]
    public PlayerController cat;
    public Transform jumpPosition;
    public float parabolicJumpHeight = 2;
    public float parabolicJumpSpeed = 1.25f;

    // Start is called before the first frame update
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0, 1, 0.5f); //Transparent Blue

        Vector3 colliderBottomPosition = transform.position;
        colliderBottomPosition.y -= GetComponent<BoxCollider>().bounds.size.y / 2;

        if (GetComponent<BoxCollider>())
            Gizmos.DrawCube(transform.position, GetComponent<BoxCollider>().bounds.size);

        if (jumpPosition != null)
        {
            Gizmos.DrawLine(colliderBottomPosition, jumpPosition.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() == cat)
        {
            cat.parabolicJumpTarget = jumpPosition;
            cat.parabolicJumpHeight = parabolicJumpHeight;
            cat.parabolicJumpSpeed = parabolicJumpSpeed;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerController>() == cat)
        {
            cat.parabolicJumpTarget = null;
        }
    }
}
