﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BasicAnimator : MonoBehaviour
{
    [Header("Frame Information")]
    public Data data;
    [Header("Sprite Information")]
    public SpriteRenderer spriteRenderer;
    [Header("Playing Information")]
    public bool playing = true;
    public bool looped = true;
    public bool disableOnEnd;
    public float frame;

    private int f;
    private int totalFrames;

    // Use this for initialization
    void Start()
    {
        totalFrames = data.frames.Count;
        if (totalFrames == 0) 
            playing = false;// sanity check
    }

    void Update()
    {
        if (playing)
        {
            //changes the frame over time
            frame += Time.deltaTime * data.frameRate;
            if (frame >= totalFrames)
            {
                //deletes script once animation is finished if script isn't intended to loop animation.
                frame -= totalFrames;
                if (!looped)
                {
                    playing = false;
                    if (disableOnEnd)
                        Destroy(this);

                    return;
                }
            }
            if (f != (int)frame)
            {
                f = (int)frame;
                spriteRenderer.sprite = data.frames[f];
            }
        }
    }

    public void Play()
    {
        frame = 0;
        playing = true;
    }
    // overload
    public void Play(Data data)
    {
        this.data = data;
        totalFrames = data.frames.Count;
        frame = 0;
        playing = true;
        if (totalFrames == 0) playing = false;// sanity check
    }

    // Holds a list of sprites and intented framerate.
    [System.Serializable]
    public class Data
    {
        public List<Sprite> frames;
        public float frameRate = 8;
    }

}