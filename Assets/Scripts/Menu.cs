using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject mainMenuHolder;
    public GameObject optionsMenuHolder;

    public Slider[] volumeSlider;
    public Toggle[] resolutionToggles;
    public Toggle fullscreenToggle;
    public int[] screenWidths;

    int activeScreenResIndex;

    private void Start()
    {
        activeScreenResIndex = PlayerPrefs.GetInt("screen res index");
        bool isFullScreen = (PlayerPrefs.GetInt("fullscreen") == 1) ? true : false;

        volumeSlider[0].value = AudioManager.instance.masterVolumePercent;
        volumeSlider[1].value = AudioManager.instance.musicVolumePercent;
        volumeSlider[2].value = AudioManager.instance.sfxVolumePercent;

        for(int i =0; i< resolutionToggles.Length; i++)
        {
            resolutionToggles[i].isOn = i == activeScreenResIndex;
        }

        fullscreenToggle.isOn = isFullScreen;
    }


    public void Play()
    {
        SceneManager.LoadScene("Game");
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void OptionsMenu()
    {
        mainMenuHolder.SetActive(false);
        optionsMenuHolder.SetActive(true);
    }
    public void MainMenu()
    {
        mainMenuHolder.SetActive(true);
        optionsMenuHolder.SetActive(false);
    }
    public void SetScreenResolution(int i)
    {
        if (resolutionToggles[i].isOn)
        {
            activeScreenResIndex = i;
            float apectRatio = 16 / 9f;
            Screen.SetResolution(screenWidths[i], (int)(screenWidths[i] / apectRatio), false);
            PlayerPrefs.SetInt("screen res index", activeScreenResIndex);
        }
    }
    public void SetFullScreen(bool isFullScreen)
    {
        for(int i = 0; i < resolutionToggles.Length; i++)
        {
            if (resolutionToggles[i].interactable == true)
            {
                resolutionToggles[i].interactable = false;
            }
            else
            {
                resolutionToggles[i].interactable = true;
            }
        }

        if (isFullScreen)
        {
            //컴퓨터 모니터의 최대 해상도를 가져와서 적용
            Resolution[] allResolutions = Screen.resolutions;
            Resolution maxResolution = allResolutions[allResolutions.Length - 1];
            Screen.SetResolution(maxResolution.width, maxResolution.height, true);
        }
        else
        {
            SetScreenResolution(activeScreenResIndex);
        }
        PlayerPrefs.SetInt("fullscreen", ((isFullScreen) ? 1: 0));
        PlayerPrefs.Save();
    }
    public void SetMasterVolume()
    {
        AudioManager.instance.SetVolume(volumeSlider[0].value, AudioManager.AudioChanel.Master);
    }
    public void SetMusicVolume()
    {
        AudioManager.instance.SetVolume(volumeSlider[1].value, AudioManager.AudioChanel.Music);
    }
    public void SetSfxVolume()
    {
        AudioManager.instance.SetVolume(volumeSlider[2].value, AudioManager.AudioChanel.Sfx);
    }
}
