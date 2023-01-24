using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum AudioChanel
    {
        Master,
        Sfx,
        Music
    }

    //마스터 볼륨
    public float masterVolumePercent { get; private set; }
    //일반 볼륨(사운드 짦은)
    public float sfxVolumePercent { get; private set; }
    //뮤직 볼륨(사운드가 긴)
    public float musicVolumePercent { get; private set; }

    AudioSource sfx2DSource;
    //뮤직소스를 배열을 사용하는 이유는
    //음악을 바꿀 때 볼륨을 조절해서 교체하려고
    AudioSource[] musicSource;
    int activeMusicSourceIndex;

    //오디오는 하나만 있으면 되기 때문에
    //싱클톤 패턴 사용
    public static AudioManager instance;

    //메인 카메라에있는 AudioListner 컴포넌트를 지우고
    //Audio mangaer자식에 게임오브젝트 안에 넣어서
    //manager에서 관리하는 이유는
    //카메라가 Player와 가까이 있지 않는 경우는
    //Player 위치에서 일어나는 소리가 안 날 수 있음(3D라서 그럼)
    //그렇다고 Player에 넣으면 Player 죽으면 소리꺼짐
    //그래서 여기서 관리
    Transform audioListener;
    Transform playerT;

    SoundLibrary soundLibrary;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }

        //싱글톤
        instance = this;

        //게임씬을 바꿔도 사라지지 않게
        DontDestroyOnLoad(gameObject);

        soundLibrary = GetComponent<SoundLibrary>();

        //여기서 두 개를 만들어서 자식으로 넣어줌
        musicSource = new AudioSource[2];
        for(int i = 0; i < 2; i++)
        {
            GameObject newMusicSource = new GameObject("Music source " + (i + 1));
            musicSource[i] = newMusicSource.AddComponent<AudioSource>();
            musicSource[i].transform.parent = transform;
        }

        GameObject newSfx2DSource = new GameObject("2D sfx source");
        sfx2DSource = newSfx2DSource.AddComponent<AudioSource>();
        newSfx2DSource.transform.parent = transform;

        //캐릭터와 오디오 리슨너 위치 같게 하려고
        audioListener = FindObjectOfType<AudioListener>().transform;

        if (FindObjectOfType<Player>() != null)
        {
            playerT = FindObjectOfType<Player>().transform;
        }
        masterVolumePercent = PlayerPrefs.GetFloat("master vol", 1f);
        sfxVolumePercent = PlayerPrefs.GetFloat("sfx vol", 1f);
        musicVolumePercent = PlayerPrefs.GetFloat("music vol", 1f);
        print(masterVolumePercent);
        print(sfxVolumePercent);
        print(musicVolumePercent);

    }

    private void Update()
    {
        //playerT 있으면 위치를 같게
        if (playerT != null)
        {
            audioListener.position = playerT.position;
        }
        else
        {
            if (FindObjectOfType<Player>() != null)
            {
                playerT = FindObjectOfType<Player>().transform;
            }
        }
    }

    public void SetVolume(float volumePercent, AudioChanel chanel)
    {
        switch (chanel)
        {
            case AudioChanel.Master:
                masterVolumePercent = volumePercent;
                break;
            case AudioChanel.Sfx:
                sfxVolumePercent = volumePercent;
                break;
            case AudioChanel.Music:
                musicVolumePercent = volumePercent;
                break;
        }

        //현재 뮤직이 재생 중일 수도 있으니
        musicSource[0].volume = musicVolumePercent * masterVolumePercent;
        musicSource[1].volume = musicVolumePercent * masterVolumePercent;
        
        PlayerPrefs.SetFloat("master vol", masterVolumePercent);
        PlayerPrefs.SetFloat("sfx vol", sfxVolumePercent);
        PlayerPrefs.SetFloat("music vol", musicVolumePercent);
        PlayerPrefs.Save();
    }

    //뮤직 재생
    public void PlayMusic(AudioClip clip, float fadeDuration = 1)
    {

        //음악 재생
        activeMusicSourceIndex = 1 - activeMusicSourceIndex;
        musicSource[activeMusicSourceIndex].clip = clip;
        musicSource[activeMusicSourceIndex].Play();

        //이건 기존의 뮤직을 서서히 줄이고
        //새로운 음악을 서서히 키우기 위해
        StartCoroutine(AnimateMusicCrossFade(fadeDuration));
    }

    //짧은 음삭 재상
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

    public void PlaySound(string name, Vector3 pos)
    {
        PlaySound(soundLibrary.GetClipFromString(name), pos);
    }

    public void PlaySound2D(string soundName)
    {
        sfx2DSource.PlayOneShot(soundLibrary.GetClipFromString(soundName), sfxVolumePercent * masterVolumePercent);
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
