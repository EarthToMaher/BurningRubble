using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    // Here in case I wanna test AudioMixers
    public AudioMixerGroup[] _mixerGroup;
    
    public Sound[] _sounds;

    // On Awake, foreach sound in _sounds, it will add an AudioSource for each clip it finds with the correct parameters
    // Set within the AudioManager
    void Awake()
    {

        foreach (Sound s in _sounds)
        {
            s._source = gameObject.AddComponent<AudioSource>();
            s._source.clip = s._clip;

            s._source.volume = s._volume;
            s._source.pitch = s._pitch;

            s._source.loop = s._loop;
            s._source.playOnAwake = s._playOnAwake;
        }
    }

    // Simple SFX script to play SFX's, this does not account for clips currently playing
    public void PlaySFX (string name)
    {
        Sound s = Array.Find(_sounds, sound => sound._name == name);
        if (s == null)
        {
            Debug.Log("Couldn't find sound: " + name);
            return;
        }
        s._source.Play();
    }

    // Simple SFX script to play SFX's, this *does* account for clips of the **Same Type** currently playing
    public void PlaySFXOnce(string name)
    {
        Sound s = Array.Find(_sounds, sound => sound._name == name);

        if (s == null)
        {
            Debug.Log("Couldn't find sound: " + name);
            return;
        }
        else if (!s._source.isPlaying)
        { 
            s._source.Play();
        }
    }
}
