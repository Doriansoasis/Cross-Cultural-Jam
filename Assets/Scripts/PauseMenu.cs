using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cursor = UnityEngine.Cursor;
using Image = UnityEngine.UI.Image;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    public TextMeshProUGUI[] questCheck = {null, null, null, null, null, null};
    private bool[] questDone = { false, false, false, false, false, false };
    [HideInInspector]
    public bool allQuestsDone = false;

    public Image image1; 
    public Image image2;
    public Color color1 = new Color(0, 0, 0, 0);
    public Color color2 = new Color(0, 0, 0, 0);
    public GameObject finalQuestUI;
    public GameObject PauseUI;
    public static bool isPaused = false;
    public GameObject endPanel;

    private bool isFinished;
    void Start()
    {
        finalQuestUI.SetActive(false);
        PauseUI.SetActive(false);
        FinishQuest(0);
    }

    void OnMouseEnter()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
            FinishQuest(0);
        
        if (Input.GetKeyDown(KeyCode.Keypad2))
            FinishQuest(1);
        
        if (Input.GetKeyDown(KeyCode.Keypad3))
            FinishQuest(2);
        
        if (Input.GetKeyDown(KeyCode.Keypad4))
            FinishQuest(3);
        
        if (Input.GetKeyDown(KeyCode.Keypad5))
            FinishQuest(4);
        
        if (Input.GetKeyDown(KeyCode.Keypad6))
            FinishQuest(5);
    }

    public void FinishQuest(int index)
    {
        questCheck[index].gameObject.SetActive(false);
        questDone[index] = true;
        
        for (int i = 0; i < questDone.Length; i++)
        {
            if (questDone[i] == false)
                return;
        }
        finalQuestUI.SetActive(true);
        allQuestsDone = true;
    }

    public void Resume()
    {
        PauseUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    void Pause()
    {
        PauseUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void EndGame()
    {
        SceneManager.LoadScene("Ending");
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("LVL_Main_Menu");
    }
}
