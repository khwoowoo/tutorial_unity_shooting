using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    float masterVolumePercent = .2f;
    float sfxVolumePercent = 1f;
    float musicVolumePercent = 1f;

    AudioSource[] musicSource;
    int activeMusicSourceIndex;

    public static AudioManager instance;

    Transform audioListener;
    Transform playerT;

    private void Awake()
    {
        instance = this;

        musicSource = new AudioSource[2];

        for(int i = 0; i < 2; i++)
        {
            GameObject newMusicSource = new GameObject("Music source " + (i + 1));
            musicSource[i] = newMusicSource.AddComponent<AudioSource>();
            musicSource[i].transform.parent = transform;
        }

        audioListener = FindObjectOfType<AudioListener>().transform;
        playerT = FindObjectOfType<Player>().transform;
    }

    private void Update()
    {
        if(playerT != null)
        {
            audioListener.position = playerT.position;
        }
    }

    public void PlayMusic(AudioClip clip, float fadeDuration = 1)
    {
        activeMusicSourceIndex = 1 - activeMusicSourceIndex;
        musicSource[activeMusicSourceIndex].clip = clip;
        musicSource[activeMusicSourceIndex].Play();

        StartCoroutine(AnimateMusicCrossFade(fadeDuration));
    }

    public void PlaySound(AudioClip clip, Vector3 pos)
    {
        //이것을 짧은 사운드를 재생할 때 좋다
        //하지만 클립이 재생되는 동안 볼륨을 조절할 수는 없다는 단점을 가지고 있다
        //초기에만 줄이기는 가능
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, pos, sfxVolumePercent * masterVolumePercent);
        }
    }

    IEnumerator AnimateMusicCrossFade(float duration)
    {
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / duration;
            musicSource[activeMusicSourceIndex].volume = Mathf.Lerp(0, musicVolumePercent * masterVolumePercent, percent);
            musicSource[1-activeMusicSourceIndex].volume = Mathf.Lerp(musicVolumePercent * masterVolumePercent, 0, percent);
            yield return null;
        }
    }
}
