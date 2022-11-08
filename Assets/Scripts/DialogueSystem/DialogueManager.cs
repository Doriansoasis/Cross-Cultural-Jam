using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum EventForTriggering
{
  None,
  OnStart,
  IsNear,
  GoesAway
};

public enum EventForClear
{
  Override,
  Time,
  GoesAway
};

public enum State
{
  QuestNotCompleted,
  QuestCompleted
};

[System.Serializable]
public class EventDialogue
{
  public EventForTriggering forTrigger;
  public State state;
  public EventForClear forClear;
  public Dialogue dialogue;
};

public class DialogueManager : MonoBehaviour
{
  public List<EventDialogue> dialogues;

  public TMP_Text displayText;

  EventDialogue actualDialogue;

  State actualState;

  public EventDialogue GetActualDialogue()
  {
    return actualDialogue;
  }

  // Start is called before the first frame update
  void Start()
  {
    displayText.text = "";
    SetState(State.QuestNotCompleted);
  }

  // Update is called once per frame
  void Update()
  {
    actualDialogue?.dialogue.Tick();
    if (actualDialogue != null)
    {
      displayText.text = actualDialogue.dialogue.getDisplayText();
      if (actualDialogue.forClear == EventForClear.Time && actualDialogue.dialogue.WaitingForClear())
      {
        ChooseDialogue(EventForTriggering.None);
      }
    }

  }

  public void SetState(State state)
  {
    actualState = state;
    ChooseDialogue(EventForTriggering.OnStart);
  }

  public void ChooseDialogue(EventForTriggering eventForTriggering)
  {
    if (eventForTriggering == EventForTriggering.None)
    {
      actualDialogue = null;
      displayText.text = "";
      return;
    }
    bool reset = true;
    bool found = false;
    foreach (EventDialogue i in dialogues)
    {
      if (i.state == actualState && i.forTrigger == eventForTriggering)
      {
        if (actualDialogue?.dialogue == i.dialogue)
        {
          reset = false;
        }
        actualDialogue = i;
        found = true;
        break;
      }

    }
    if (reset && found)
    {
      actualDialogue?.dialogue.Reset();
    }

  }
}