using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public AudioClip mainTheme;
    public AudioClip menuTheme;

    string sceneName;

    // Start is called before the first frame update
    void Start()
    {
        OnLevelWasLoaded(0);
    }

    private void OnLevelWasLoaded(int level)
    {
        print(level);
        string newSceneName = SceneManager.GetActiveScene().name;

        if(newSceneName != sceneName)
        {
            sceneName = newSceneName;
            Invoke("PlayMusic", 2);
        }
    }

    void PlayMusic()
    {
        AudioClip clip = null;

        if(sceneName == "Menu")
        {
            clip = menuTheme;
        }
        else if (sceneName == "Game")
        {
            clip = mainTheme;
        }

        if(clip != null)
        {
            AudioManager.instance.PlayMusic(clip, 2);
            Invoke("PlayMusic", clip.length);
        }
    }
}
