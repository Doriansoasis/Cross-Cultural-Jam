using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAwayDialogue : MonoBehaviour
{
  public DialogueManager dialogues;

  // Start is called before the first frame update
  void Start()
  {
      
  }

  // Update is called once per frame
  void Update()
  {
      
  }

  void OnTriggerExit(Collider col)
  {
    if (col.gameObject.name == "Player")
    {
      if (
        dialogues.GetActualDialogue().forClear == EventForClear.GoesAway &&
        dialogues.GetActualDialogue().dialogue.HasFinished()
        )
      {
        dialogues.ChooseDialogue(EventForTriggering.None);
      }
      else
      {
        dialogues.ChooseDialogue(EventForTriggering.GoesAway);
      }
    }
  }
}
