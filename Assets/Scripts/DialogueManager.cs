using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum EventForTriggering { 
  OnStart,
  IsNear,
  GoesAway
};
[System.Serializable]
public class EventDialogue
{
  public EventForTriggering forTrigger;
  public Dialogue dialogue;
};

public class DialogueManager : MonoBehaviour
{
  public List<EventDialogue> dialogues;

  public TMP_Text displayText;

  Dialogue actualDialogue;
  // Start is called before the first frame update
  void Start()
  {
    displayText.text = "";
    ChooseDialogue(EventForTriggering.OnStart) ;
  }

  // Update is called once per frame
  void Update()
  {
    //actualDialogue.
    actualDialogue?.Tick();
    if(actualDialogue != null)
    {
      displayText.text = actualDialogue.displayText;
    }
  }

  void ChooseDialogue(EventForTriggering eventForTriggering)
  {
    foreach(EventDialogue i in dialogues)
    {
      if(i.forTrigger == eventForTriggering)
      {
        actualDialogue = i.dialogue;
      }

    }
    //actualDialogue = dialogues[EventForTriggering.OnStart];
    actualDialogue?.Reset();
  }
}
