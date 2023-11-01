using System;
using UnityEngine;
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;
    public Action setCinematicSound;
    Sound _currentMusic;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    private void Start()
    {
        PlayMusic("Music");
    }
    public void PlayMusic(string name)
    {
        if (_currentMusic != null && _currentMusic.soundName.Equals(name)) return;

        Sound s = Array.Find(musicSounds, x => x.soundName == name);
        _currentMusic = s;

        if (s == null)
            Debug.Log("Sound Not Found!");
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }
    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.soundName == name);
        if (s == null)
            Debug.Log("Sound Not Found");
        else
            sfxSource.PlayOneShot(s.clip);
    }
}
