using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BlindBehavior : MonoBehaviour
{
    private bool foundHouse;
    public float HouseAcceptanceRadius = 20f;
    public Transform HousePosition;
    
    public PlayerController dog;
    public float hearingDistance;
    private bool noticedBarking = false;
    private PauseMenu pausemenu;
    
    private Vector3 destination;
    private Vector3 vecToDestination;
    private bool hasDestination = false;
    
    public float speed = 2;
    
    public GameObject spawnedMeat;
    
    private bool isRotating = false;
    private float angleToDestination;
    public DialogueManager dialogues;

  void Start()
    {
        pausemenu = GameObject.Find("PauseMenu").GetComponent<PauseMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (dog.isBarking && !noticedBarking)
        //    HearBark();

        //else if (!dog.isBarking && noticedBarking)
        //    noticedBarking = false;
        
        if (isRotating && hasDestination)
            Rotate();
        
        else if (hasDestination && !foundHouse)
            GoToDestination();

        if (!foundHouse && Vector3.Distance(HousePosition.position, transform.position) < HouseAcceptanceRadius)
        {
            foundHouse = true;
            destination = HousePosition.position;
            SetRotation();
            pausemenu.FinishQuest(3);
            dialogues.SetState(State.QuestCompleted);
            Instantiate(spawnedMeat, new Vector3(transform.position.x, transform.position.y, transform.position.z) + transform.forward*2, transform.rotation);
        }
    }

    void Rotate()
    {
        transform.Rotate(Vector3.up, angleToDestination/30);
        if (Vector3.Angle(vecToDestination, transform.forward) <= 5.0)
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

    public void OnBark()
    {
        if (Vector3.Distance(transform.position, dog.transform.position) <= hearingDistance)
        {
            destination = dog.transform.position;
            SetRotation();
            hasDestination = true;
            if(dialogues.GetState() == State.QuestNotCompleted)
            {
              dialogues.SetState(State.Stage1);
            }
            
            //noticedBarking = true;
        }
    }

    void SetRotation()
    {
        vecToDestination = destination - transform.position;
        angleToDestination = Vector3.SignedAngle(transform.forward, vecToDestination, Vector3.up);
        Debug.Log(angleToDestination);
        Debug.Log(vecToDestination);
        isRotating = true;
    }
}
