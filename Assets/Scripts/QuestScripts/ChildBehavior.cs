using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class ChildBehavior : MonoBehaviour
{
    private PauseMenu pausemenu;
    private NPC_GrabItem npcItemHandler;
    public Vector3 ThrowDirection1;
    public Vector3 ThrowDirection2;
    public Vector3 ThrowDirection3;
    public Vector3 ThrowDirection4;

    private int nbStickThrown = 0;
    void Start()
    {
        pausemenu = GameObject.Find("PauseMenu").GetComponent<PauseMenu>();
        npcItemHandler = this.GetComponent<NPC_GrabItem>();
        //Debug.Log((npcItemHandler.speed));
    }
    void Update()
    {
        if (npcItemHandler.isHoldingItem
            && this.transform.position == npcItemHandler.originP
            && npcItemHandler.canGrab)
        {
            if (npcItemHandler.pickedObject == npcItemHandler.keyitems[0])
            {
                npcItemHandler.isHoldingItem = false;
                ThrowItem();
                nbStickThrown++;
            }
            else
            {
                pausemenu.FinishQuest(0);
            }
        }
    }

    void ThrowItem()
    {
        //Debug.Log("Throw");
        switch (nbStickThrown)
        {
            case 0:
                npcItemHandler.ThrowObject(2,ThrowDirection1);
                break;
            case 1:
                npcItemHandler.ThrowObject(2,ThrowDirection2);
                break;
            case 2:
                npcItemHandler.ThrowObject(2,ThrowDirection3);
                break;
            default:
                npcItemHandler.ThrowObject(2,ThrowDirection4);
                break;
        }
    }
}
