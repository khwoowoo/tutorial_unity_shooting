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

    //������ ����
    public float masterVolumePercent { get; private set; }
    //�Ϲ� ����(���� �F��)
    public float sfxVolumePercent { get; private set; }
    //���� ����(���尡 ��)
    public float musicVolumePercent { get; private set; }

    AudioSource sfx2DSource;
    //�����ҽ��� �迭�� ����ϴ� ������
    //������ �ٲ� �� ������ �����ؼ� ��ü�Ϸ���
    AudioSource[] musicSource;
    int activeMusicSourceIndex;

    //������� �ϳ��� ������ �Ǳ� ������
    //��Ŭ�� ���� ���
    public static AudioManager instance;

    //���� ī�޶��ִ� AudioListner ������Ʈ�� �����
    //Audio mangaer�ڽĿ� ���ӿ�����Ʈ �ȿ� �־
    //manager���� �����ϴ� ������
    //ī�޶� Player�� ������ ���� �ʴ� ����
    //Player ��ġ���� �Ͼ�� �Ҹ��� �� �� �� ����(3D�� �׷�)
    //�׷��ٰ� Player�� ������ Player ������ �Ҹ�����
    //�׷��� ���⼭ ����
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

        //�̱���
        instance = this;

        //���Ӿ��� �ٲ㵵 ������� �ʰ�
        DontDestroyOnLoad(gameObject);

        soundLibrary = GetComponent<SoundLibrary>();

        //���⼭ �� ���� ���� �ڽ����� �־���
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

        //ĳ���Ϳ� ����� ������ ��ġ ���� �Ϸ���
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
        //playerT ������ ��ġ�� ����
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

        //���� ������ ��� ���� ���� ������
        musicSource[0].volume = musicVolumePercent * masterVolumePercent;
        musicSource[1].volume = musicVolumePercent * masterVolumePercent;
        
        PlayerPrefs.SetFloat("master vol", masterVolumePercent);
        PlayerPrefs.SetFloat("sfx vol", sfxVolumePercent);
        PlayerPrefs.SetFloat("music vol", musicVolumePercent);
        PlayerPrefs.Save();
    }

    //���� ���
    public void PlayMusic(AudioClip clip, float fadeDuration = 1)
    {

        //���� ���
        activeMusicSourceIndex = 1 - activeMusicSourceIndex;
        musicSource[activeMusicSourceIndex].clip = clip;
        musicSource[activeMusicSourceIndex].Play();

        //�̰� ������ ������ ������ ���̰�
        //���ο� ������ ������ Ű��� ����
        StartCoroutine(AnimateMusicCrossFade(fadeDuration));
    }

    //ª�� ���� ���
    public void PlaySound(AudioClip clip, Vector3 pos)
    {
        //�̰��� ª�� ���带 ����� �� ����
        //������ Ŭ���� ����Ǵ� ���� ������ ������ ���� ���ٴ� ������ ������ �ִ�
        //�ʱ⿡�� ���̱�� ����
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
