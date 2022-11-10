using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenerBehavior : MonoBehaviour
{
    private PauseMenu pausemenu;

    public WateredPlant[] plants;
    public Transform[] weeds;
    public float WeedDistanceAcceptable = 10f;
    void Start()
    {
        pausemenu = GameObject.Find("PauseMenu").GetComponent<PauseMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < plants.Length; i++)
        {
            if (!plants[i].isWatered)
                return;
        }
        
        for (int i = 0; i < weeds.Length; i++)
        {
            if (Vector3.Distance(weeds[i].position, transform.position) <= WeedDistanceAcceptable)
                return;
        }
        
        pausemenu.FinishQuest(5);
    }
}
