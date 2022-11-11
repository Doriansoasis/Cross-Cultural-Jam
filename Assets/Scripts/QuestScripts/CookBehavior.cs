using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookBehavior : MonoBehaviour
{
    private PauseMenu pausemenu;
    private NPC_GrabItem npcItemHandler;

    private bool hasSpice = false;
    public Transform SpiceBottle;
    public float SpiceDistanceAcceptability = 10f;
    
    private int nbIngredients = 0;
    public int ingredientsMax = 4;
    private bool questOver = false;

    void Start()
    {
        pausemenu = GameObject.Find("PauseMenu").GetComponent<PauseMenu>();
        npcItemHandler = this.GetComponent<NPC_GrabItem>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(nbIngredients);
        if (npcItemHandler.isHoldingItem
            && this.transform.position == npcItemHandler.originP)
        {
            nbIngredients++;
            npcItemHandler.DestroyHeldItem();
        }

        if (SpiceBottle != null && Vector3.Distance(transform.position, SpiceBottle.position) < SpiceDistanceAcceptability && !hasSpice)
        {
            nbIngredients++;
            hasSpice = true;
        }

        if (nbIngredients == ingredientsMax && !questOver)
        {
            pausemenu.FinishQuest(4);
            questOver = true;
        }
    }
}
