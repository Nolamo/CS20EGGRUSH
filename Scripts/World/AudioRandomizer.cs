using System.Collections.Generic;
using UnityEngine;

// used for the day night cycle.
public class AudioRandomizer : MonoBehaviour
{
    public AudioSource audioSource;

    public List<AudioSource> audioClips = new List<AudioSource>();

    private void Start()
    {
        RandomizeClips();
    }

    // picks a random clip from the list and plays it
    public void RandomizeClips()
    {
        audioSource = audioClips[Random.Range(0, audioClips.Count)];
        for (int i = 0; i < audioClips.ToArray().Length; i++) 
        {
            if (audioClips[i] != audioSource) //If the audio  source is not the chosen source, mute it.
                audioClips[i].volume = 0;
        }
    }
}