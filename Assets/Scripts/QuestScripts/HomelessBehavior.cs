using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomelessBehavior : MonoBehaviour
{
    private PauseMenu pausemenu;
    
    public float HangoutTime = 30;
    public float HangoutTimeEnding = 5;
    private float timer = 0;
    public Transform dog;
    public float HangoutMaxDistance = 10;
    private bool questOver = false;
    private bool allQuestsOver = false;
    private bool stopUpdate = false;
    public DialogueManager dialogues;
    void Start()
    {
        pausemenu = GameObject.Find("PauseMenu").GetComponent<PauseMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        if (stopUpdate)
            return;
    if (dialogues.GetState() == State.QuestCompleted && dialogues.GetActualDialogue().dialogue.HasFinished())
    {
      Debug.Log("a");
    }
      if (dialogues.GetState() == State.QuestCompleted && dialogues.GetActualDialogue().dialogue.HasFinished())
        {
          Sleep();
        }

        if (Vector3.Distance(dog.position, transform.position) <= HangoutMaxDistance)
        {
            timer += Time.deltaTime;
        }
        
        //if (timer >= HangoutTime && !questOver)
        //{
        //    pausemenu.FinishQuest(2);
        //    questOver = true;
        //}
        
        if (pausemenu.allQuestsDone && !allQuestsOver)
        {
            allQuestsOver = true;
            timer = 0;
        }
        
        if (timer >= HangoutTimeEnding && allQuestsOver)
        {
            Debug.Log("Game Over");
            stopUpdate = true;
            //game over
        }
    }

  public void OnBark()
  {
    if (dialogues.GetActualDialogue().dialogue.HasFinished())
    {
      if(dialogues.GetState() == State.QuestNotCompleted)
      {
        dialogues.SetState(State.Stage1);
      }
      else if (dialogues.GetState() == State.Stage1)
      {
        dialogues.SetState(State.QuestCompleted);
      }
    }
  }

  void Sleep()
  {
    transform.rotation = Quaternion.Euler(90, 0, 0);
    pausemenu.FinishQuest(2);
    questOver = true;
  }
}
