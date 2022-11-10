using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class ChildBehavior : MonoBehaviour
{
    private PauseMenu pausemenu;
    private NPC_GrabItem npcItemHandler;

    private int nbStickThrown = 0;
    void Start()
    {
        pausemenu = GameObject.Find("PauseMenu").GetComponent<PauseMenu>();
        npcItemHandler = this.GetComponent<NPC_GrabItem>();
        Debug.Log((npcItemHandler.speed));
    }
    void Update()
    {
        if (npcItemHandler.isHoldingItem
            && this.transform.position == npcItemHandler.originP
            && this.transform.eulerAngles == npcItemHandler.originR)
        {
            if (npcItemHandler.pickedObject == npcItemHandler.keyitems[0])
            {
                npcItemHandler.pickedObject.Drop();
                npcItemHandler.isHoldingItem = false;
                nbStickThrown++;
                ThrowItem(true);
            }
            else
            {
                npcItemHandler.pickedObject.Drop();
                npcItemHandler.isHoldingItem = false;
                pausemenu.FinishQuest(0);
                ThrowItem(true);
            }
        }
    }

    void ThrowItem(bool isStick)
    {
        
    }
}
