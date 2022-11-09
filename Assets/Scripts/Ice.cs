using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour
{
    public float meltTimer;
    private Vector3 origin;
    private bool startedMelting;
    private float timer;
    private PickableObject objectRef;
    public NPC_GrabItem workerRef;
    void Start()
    {
        origin = transform.position;
        objectRef = GetComponent<PickableObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!startedMelting && objectRef.isHeld && !workerRef.isHoldingItem)
            startedMelting = true;
        
        else if (workerRef.isHoldingItem)
            startedMelting = false;
        
        if (startedMelting)
        {
            timer += Time.deltaTime;
        }

        if (timer >= meltTimer)
        {
            transform.position = origin;
        }
    }
}
