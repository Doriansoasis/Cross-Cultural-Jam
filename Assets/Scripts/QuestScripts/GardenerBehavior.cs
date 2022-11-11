using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenerBehavior : MonoBehaviour
{
    private PauseMenu pausemenu;

    public WateredPlant[] plants;
    public Transform[] weeds;
    public float WeedDistanceAcceptable = 10f;
    public DialogueManager dialogues;
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
            //Debug.Log(Vector3.Distance(weeds[i].position, transform.position));
            if (Vector3.Distance(weeds[i].position, transform.position) < WeedDistanceAcceptable)
                return;
        }
        dialogues.SetState(State.QuestCompleted);
        pausemenu.FinishQuest(5);
    }
}
