using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearDialogue : MonoBehaviour
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

  void OnTriggerEnter(Collider col)
  {
    Debug.Log(col.gameObject.name);
    if (col.gameObject.name == "PlayerDog")
    {
      Debug.Log("near");
      dialogues.ChooseDialogue(EventForTriggering.IsNear);
    }
  }
}