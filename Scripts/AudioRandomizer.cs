using System.Collections.Generic;
using UnityEngine;

public class AudioRandomizer : MonoBehaviour
{
    public AudioSource audioSource;

    public List<AudioSource> audioClips = new List<AudioSource>();

    private void Start()
    {
        RandomizeClips();
    }

    public void RandomizeClips()
    {
        audioSource = audioClips[Random.Range(0, audioClips.Count)];
        for (int i = 0; i < audioClips.ToArray().Length; i++)  //The "iBall" for-loop Goes through all of the Array.
        {
            if (audioClips[i] != audioSource) //If "iBall" for-loop calls the current object, skip it.
            {
                audioClips[i].volume = 0;
            }
        }
    }
}