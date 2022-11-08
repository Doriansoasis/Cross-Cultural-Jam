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

  public void ChooseDialogue(EventForTriggering eventForTriggering)
  {
    bool reset = true;
    foreach (EventDialogue i in dialogues)
    {
      if (i.forTrigger == eventForTriggering)
      {
        if (actualDialogue == i.dialogue)
        {
          reset = false;
        }
        actualDialogue = i.dialogue;
      }

    }
    //actualDialogue = dialogues[EventForTriggering.OnStart];
    if (reset)
    {
      actualDialogue?.Reset();
    }
  }
}
