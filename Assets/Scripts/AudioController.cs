using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
[System.Serializable]

public class AudioCategory
{
    [SerializeField]
    public string name;
    [SerializeField]
    [Range(1,2)]
    public float pitchVariation = 1;
    [SerializeField]
    public AudioClip[] clips;
}
public class AudioPlayer
{
    public AudioClip[] clips;
    public float pitchVariation;
    public AudioSource audioSource;

    public AudioPlayer(AudioClip[] clips, float pitchVariation, AudioSource audioSource)
    {
        this.clips = clips;
        this.pitchVariation = pitchVariation;
        this.audioSource = audioSource;
    }

    public void Play(float volume = 1f)
    {
        audioSource.clip = clips[Random.Range(0, clips.Length)];
        audioSource.volume = volume;
        audioSource.pitch = Random.Range(1 / pitchVariation, 1 * pitchVariation);
        audioSource.Play();
    }
}

public class AudioController : MonoBehaviour
{
    public static AudioController instance;

    public AudioSource sfxSourcePrefab;

    [SerializeField] AudioCategory[] categories;

    protected static Dictionary<string, AudioPlayer> sourceDict = new();

    private void Awake()
    {
        if (!instance) instance = this;
        else
        {
            Destroy(gameObject);
            enabled = false;
            return;
        }
        DontDestroyOnLoad(gameObject);
        foreach (AudioCategory category in categories)
        {
            string name = category.name.ToLower();
            sourceDict[name] = new AudioPlayer(category.clips, category.pitchVariation, Instantiate(sfxSourcePrefab, transform));
        }
    }

    public void Play(string sfx = "", float volume = 1f)
    {
        if (!instance)
        {
            Debug.LogWarning("Could not play sound since the AudioManager gameobject hasn't been loaded yet!");
            return;
        }

        sfx = sfx.ToLower();

        if (!sourceDict.ContainsKey(sfx)) return;

        sourceDict[sfx].Play(volume);
    }
}
