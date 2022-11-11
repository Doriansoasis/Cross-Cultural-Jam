using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Image = UnityEngine.UI.Image;

public class Ending : MonoBehaviour
{
    public Image image;
    private float opacity = 0f;
    private Color color = new Color(1, 1, 1, 0);
    void Start()
    {
        image.color = color;
    }

    // Update is called once per frame
    void Update()
    {
        color.a += 0.005f;
        image.color = color;
        if (color.a >= 1f)
        {
            if (Input.anyKeyDown)
            {
                SceneManager.LoadScene("LVL_Main_Menu");
            }
        }
    }
}
