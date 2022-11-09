using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerBehavior : MonoBehaviour
{
    private PauseMenu pausemenu;
    private NPC_GrabItem npcItemHandler;
    private bool questOver = false;
    void Start()
    {
        pausemenu = GameObject.Find("PauseMenu").GetComponent<PauseMenu>();
        npcItemHandler = this.GetComponent<NPC_GrabItem>();
        Debug.Log((npcItemHandler.speed));
    }

    // Update is called once per frame
    void Update()
    {
        if (npcItemHandler.isHoldingItem
            && this.transform.position == npcItemHandler.originP
            && this.transform.rotation == npcItemHandler.originR
            && !questOver)
        {
            pausemenu.FinishQuest(1);
            questOver = true;
        }
    }
}
