using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BlindBehavior : MonoBehaviour
{
    public Transform HousePosition;
    public GameObject dog;
    public float hearingDistance;
    private PauseMenu pausemenu;
    private Vector3 destination;
    private bool hasDestination;
    public float speed = 2;
    private bool foundHouse;
    public float HouseAcceptanceRadius = 20f;
    private bool isRotating = false;
    private float angleToDestination;
    void Start()
    {
        pausemenu = GameObject.Find("PauseMenu").GetComponent<PauseMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dog == null || HousePosition == null)
            return;
        if (isRotating)
            Rotate();
        
        else if (hasDestination)
            GoToDestination();

        if (!foundHouse && Vector3.Distance(HousePosition.position, transform.position) < HouseAcceptanceRadius)
        {
            foundHouse = true;
            destination = HousePosition.position;
            SetRotation();
            pausemenu.FinishQuest(3);
        }
    }

    void Rotate()
    {
        transform.Rotate(Vector3.up, angleToDestination/30);
        if (Vector3.Angle(destination, transform.forward) <= 5.0)
        {
            transform.LookAt(destination);
            isRotating = false;
        }
    }

    void GoToDestination()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, speed);
        
        if (Vector3.Distance(transform.position, destination) <= 1.0f)
        {
            hasDestination = false;
        }
    }

    void HearBark()
    {
        if (Vector3.Distance(transform.position, dog.transform.position) <= hearingDistance)
        {
            destination = dog.transform.position;
            SetRotation();
            hasDestination = true;
        }
    }

    void SetRotation()
    {
        Vector3 vecToDestination = destination - transform.position;
        angleToDestination = Vector3.SignedAngle(transform.forward, vecToDestination, Vector3.up);
        isRotating = true;
    }
}
