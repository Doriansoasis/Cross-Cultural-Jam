using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audiomixer;
    Resolution[] resolutions;

    public TMPro.TMP_Dropdown resDropdown;

    void Start()
    {
        resolutions = Screen.resolutions;

        resDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResIndex = 0;

        for(int i = 0; i< resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if ((resolutions[i].width == Screen.currentResolution.width) && (resolutions[i].height == Screen.currentResolution.height))
            {
                currentResIndex = i;
            }
        }
        resDropdown.AddOptions(options);

        resDropdown.value = currentResIndex;
        resDropdown.RefreshShownValue();
    }


    public void SetVolume(float value)
    {
        audiomixer.SetFloat("MainVolume", value);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int choice)
    {
        Screen.SetResolution(resolutions[choice].width, resolutions[choice].height, Screen.fullScreen);
    }
}
