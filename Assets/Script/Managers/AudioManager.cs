using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private List<AudioSource> sfxSources;
    private AudioSource voiceSource;
    private AudioSource musicSource;

    [SerializeField, Tooltip("The Amount of Available SFX Sources")]
    private int numberOfSFXSources = 5; 

    public float SfxVolume { get; set; } = .5f;
    public float VoiceVolume { get; set; } = .5f;
    public float MusicVolume { get; set; } = .5f;
    public float MasterVolume { get; set; } = .5f;

    private Dictionary<string, AudioClip> sfxClipsDict;
    private Dictionary<string, AudioClip> voiceClipsDict;
    private Dictionary<string, AudioClip> musicClipsDict;

    // Ludwig said the music was too loud by default - Johan
    // You can change the individual music clip to a lower volume in the inspector!!! >:3 - Evil Andreas
    // But fine, I'll let this const slide... for now - Less Evil Andreas
    private const float musicVolumeFactor = .45f;

    private void Awake()
    {
        sfxSources = new List<AudioSource>();
        for (int i = 0; i < numberOfSFXSources; i++) // or any other number of sources you want to use
        {
            sfxSources.Add(gameObject.AddComponent<AudioSource>());
        }

        voiceSource = gameObject.AddComponent<AudioSource>();

        musicSource = gameObject.AddComponent<AudioSource>();

        // Populate the sfx clips dictionary
        sfxClipsDict = new Dictionary<string, AudioClip>();
        AudioClip[] sfxClips = Resources.LoadAll<AudioClip>("Audio/SFX");
        foreach (AudioClip clip in sfxClips)
        {
            sfxClipsDict.Add(clip.name, clip);
        }

        // Populate the voice clips dictionary
        voiceClipsDict = new Dictionary<string, AudioClip>();
        AudioClip[] voiceClips = Resources.LoadAll<AudioClip>("Audio/Voice");
        foreach (AudioClip clip in voiceClips)
        {
            voiceClipsDict.Add(clip.name, clip);
        }

        // Populate the music clips dictionary
        musicClipsDict = new Dictionary<string, AudioClip>();
        AudioClip[] musicClips = Resources.LoadAll<AudioClip>("Audio/Music");
        foreach (AudioClip clip in musicClips)
        {
            musicClipsDict.Add(clip.name, clip);
        }
    }

    public void PlaySfx(string clipName, float pitch = 1f)
    {
        AudioClip clip;
        if(sfxClipsDict.TryGetValue(clipName, out clip))
        {
            AudioSource source = GetAvailableSfxSource();
            source.pitch = pitch;
            source.PlayOneShot(clip, SfxVolume * MasterVolume);
        }
        else
        {
            Debug.LogWarning($"Audio clip {clipName} not found in sfx dictionary.");
        }
    }

    public void PlayVoice(string clipName, float pitch = 1f)
    {
        AudioClip clip;
        if (sfxClipsDict.TryGetValue(clipName, out clip))
        {
            voiceSource.pitch = pitch;
            voiceSource.PlayOneShot(clip, VoiceVolume * MasterVolume);
        }
        else
        {
            Debug.LogWarning($"Audio clip {clipName} not found in voicec dictionary.");
        }
    }

    public void PlayLoopingSfx(string clipName, Func<bool> condition)
    {
        AudioClip clip;
        if (sfxClipsDict.TryGetValue(clipName, out clip))
        {
            StartCoroutine(PlayLoopingSfxCoroutine(clip, condition));
        }
        else
        {
            Debug.LogWarning($"Audio clip {clipName} not found in sfx dictionary.");
        }
    }

    private IEnumerator PlayLoopingSfxCoroutine(AudioClip clip, Func<bool> condition)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.loop = true;
        source.volume = SfxVolume * MasterVolume;
        source.Play();

        while (condition())
        {
            yield return null;
        }

        source.Stop();
        Destroy(source);
    }

    public void PlayMusic(string clipName, bool loop = true, float pitch = 1f)
    {
        AudioClip clip;
        if (musicClipsDict.TryGetValue(clipName, out clip))
        {
            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.pitch = pitch;
            musicSource.volume = MusicVolume * MasterVolume * musicVolumeFactor;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning($"Audio clip {clipName} not found in music dictionary.");
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PauseAudio()
    {
        sfxSources.ForEach(source => source.Pause());
        voiceSource.Pause();
        musicSource.Pause();
    }

    public void UnPauseAudio()
    {
        sfxSources.ForEach(source => source.UnPause());
        voiceSource.UnPause();
        musicSource.UnPause();
    }

    public void ChangeAudioSettings(float master, float music, float sfx, float voice)
    {
        MasterVolume = master;
        MusicVolume = music;
        SfxVolume = sfx;
        VoiceVolume = voice;

        musicSource.volume = MusicVolume * MasterVolume * musicVolumeFactor;
    }

    public string GetCurrentMusic()
    {

        string result;

        try
        {
            result = musicSource.clip.name;
        }
        catch
        {
            result = "Noel";
        }
        
        return result;
    }

    private AudioSource GetAvailableSfxSource()
    {
        foreach (AudioSource source in sfxSources)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }

        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        sfxSources.Add(newSource);
        return newSource;
    }
}
