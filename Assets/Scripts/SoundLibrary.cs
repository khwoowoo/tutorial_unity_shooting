using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLibrary : MonoBehaviour
{
    public SoundGroup[] soundGroups;

    Dictionary<string, AudioClip[]> gourpDictionary = new Dictionary<string, AudioClip[]>();

    private void Awake()
    {
        foreach(SoundGroup soud in soundGroups)
        {
            gourpDictionary.Add(soud.groupID, soud.group);
        }
    }

    public AudioClip GetClipFromString(string name)
    {
        if (gourpDictionary.ContainsKey(name))
        {
            AudioClip[] sounds = gourpDictionary[name];
            return sounds[Random.Range(0, sounds.Length)];
        }
        return null;
    }

    [System.Serializable]
    public class SoundGroup
    {
        public string groupID;
        public AudioClip[] group;
    }
}
