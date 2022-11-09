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
    void Start()
    {
        pausemenu = GameObject.Find("PauseMenu").GetComponent<PauseMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dog == null)
            return;
        if (Vector3.Distance(dog.position, transform.position) <= HangoutMaxDistance)
        {
            timer += Time.deltaTime;
        }
        
        if (timer >= HangoutTime && !questOver)
        {
            pausemenu.FinishQuest(2);
            questOver = true;
        }

        if (pausemenu.allQuestsDone && !allQuestsOver)
        {
            allQuestsOver = true;
            timer = 0;
        }

        if (timer >= HangoutTimeEnding && allQuestsOver)
        {
            //game over
        }
    }
}
