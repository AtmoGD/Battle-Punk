using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public List<Sound> sounds = new List<Sound>();

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.volume = sound.volume;
            sound.source.clip = sound.clip;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }

    public void Play(string _name)
    {
        Sound s = sounds.Find(sound => sound.name == _name);
        if (s == null) return;
        s.source.Play();
    }
}
