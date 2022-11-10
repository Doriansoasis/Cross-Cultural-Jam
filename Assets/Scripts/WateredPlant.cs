using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateredPlant : MonoBehaviour
{
    [HideInInspector]
    public bool isWatered = false;
    public Transform wateringCan;
    public float wateredDistance = 1;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isWatered && Vector3.Distance(transform.position, wateringCan.position) < wateredDistance)
        {
            Debug.Log("Watered");
            isWatered = true;
        }
    }
}
