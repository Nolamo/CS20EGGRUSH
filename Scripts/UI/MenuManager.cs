﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof((PlayerMovement, WaveManager)))]
public class MenuManager : MonoBehaviour
{
    [Header("Systems & Classes")]
    public PlayerMovement playerMovement;
    public WaveManager waveManager;
    public GameObject overlay;
    public Animator menuAnimator;
    public GameObject menuClass;
    public GameObject endScreenClass;
    public GameObject pauseScreenClass;

    [Header("Audio")]
    [SerializeField] private AudioSource deathJingleDay;
    [SerializeField] private AudioSource deathJingleNight;
    [SerializeField] private AudioSource pauseJingle;
    [SerializeField] private AudioSource winJingle;
    [HideInInspector]
    public bool jinglePlaying = false;
    
    [Header("Misc")]
    [SerializeField] private bool CoroutineStarted;
    public bool GameStarted;
    public bool GamePaused;
    bool overlayFinished;
    bool endScreenViewed;
    public float GameTime;
    public int GameDeaths;
    [SerializeField] Text timerText;
    [SerializeField] Text damageText;
    [HideInInspector]
    public int damageTaken;
    [SerializeField] Text selfHarmText;
    [HideInInspector]
    public int selfHarmTaken;
    [SerializeField] Text deathText;

    string minutesString;
    string secondsString;
    string hoursString;
    int seconds;
    int minutes;
    int hours;

    void Start()
    {
        StartCoroutine(OverlayWait());
    }

    void Update()
    {
        // Used on the main menu, starts the game when any button is pressed.
        if (Input.anyKey && overlayFinished && !CoroutineStarted)
        {
            DoOnce();
            StartCoroutine(DisableMenu());
        }
        // Used on the end screen, starts the game when any button is pressed.
        if (Input.anyKey && endScreenViewed)
            StartCoroutine(EndScreenFade());
        // Pause/Unpause the game
        if (Input.GetButtonDown("Pause"))
            PauseMenu(GamePaused);

        // Timer Stuff
        {
            seconds = (int)GameTime % 60;
            minutes = (int)GameTime / 60;
            hours = (int)GameTime / 3600;

            if (GameStarted) GameTime += Time.unscaledDeltaTime;

            // Ints to String formatting. Used for in-game timer.
            if (minutes == 0) minutesString = "00";
            else if (minutes < 10) minutesString = "0" + minutes.ToString();
            else minutesString = minutes.ToString();
            if (minutes == 60) minutesString = "00";
            if (hours == 0) hoursString = "00";
            if (hours < 10) hoursString = "0" + hours.ToString();
            else hoursString = hours.ToString();
            if (seconds == 0) secondsString = "00";
            else if (seconds < 10) secondsString = "0" + seconds.ToString();
            else secondsString = seconds.ToString();
        }
    }

    /// <summary> called at the beginning of a coroutine, last minute patch to a bug caused by oversight and lack of planning. </summary>
    void DoOnce()
    {
        if (!CoroutineStarted)
        {
            CoroutineStarted = true;
            menuAnimator.Play("MainMenuTrigger");
        }
    }

    /// <summary> Enables the main menu. Used for reseting after credits & player death. </summary>
    /// <param name="Died"> Death logic, only use when calling a player's death. </param>
    public IEnumerator EnableMenu(bool Died)
    {
        GameStarted = false;
        if(Died)
        {
            GameDeaths += 1;
            StartCoroutine(MusicDampen(deathJingleDay));
            StartCoroutine(MusicDampen(deathJingleNight));
            deathJingleDay.Play();
            deathJingleNight.Play();
        }
        playerMovement.shake.GameEnded = true;
        waveManager.waveText.text = "";
        waveManager.playAutomatically = false;
        waveManager.playing = false;
        waveManager.waveStarted = false;
        waveManager.finishedSpawning(waveManager.spawners);
        foreach (GameObject i in GameObject.FindGameObjectsWithTag("Upgrade"))
        {
            if(i != null)
            {
                Destroy(i);
            }
        }
        for (int i = 0; i < waveManager.spawners.Length; i++)
        {
            waveManager.spawners[i].RestartWave();
        }
        waveManager.KillAllEntities();
        //Clear spawned items
        foreach (GameObject i in waveManager.randomizer.spawnedItems)
        {
            if (i != null)
                Destroy(i);
        }
        waveManager.curWave = 1;
        waveManager.firstWave = true;
        menuAnimator.Play("MainMenuEnable");
        playerMovement.ResetPlayer(true);
        yield return new WaitForSeconds(2);
        CoroutineStarted = false;
    }

    /// <summary> Disables the menu and starts gameplay. </summary>
    IEnumerator DisableMenu()
    {
        yield return new WaitForSeconds(1);
        waveManager.delay = 4;
        waveManager.playAutomatically = true;
        GameStarted = true;
        playerMovement.shake.GameEnded = false;
        yield return new WaitForSeconds(1);
        menuClass.SetActive(false);
    }

    /// <summary> Credits Screen. Resets stats. </summary>
    public IEnumerator EndScreen()
    {
        menuClass.SetActive(false);
        endScreenClass.SetActive(true);
        GameStarted = false;
        playerMovement.shake.GameEnded = true;
        waveManager.waveText.text = "";
        //StartCoroutine(MusicDampen(winJingle));
        waveManager.playAutomatically = false;
        waveManager.playing = false;
        waveManager.waveStarted = false;
        timerText.text = hours + ":" + minutesString + ":" + secondsString;
        damageText.text = damageTaken.ToString();
        selfHarmText.text = selfHarmTaken.ToString();
        deathText.text = GameDeaths.ToString();
        winJingle.Play();
        menuAnimator.Play("EndScreen");
        yield return new WaitForSeconds(3);
        endScreenViewed = true;
        hours = 0;
        minutes = 0;
        seconds = 0;
    }

    /// <summary> Fades out the Endscreen. Triggered when the player presses any button during the endscreen. </summary>
    IEnumerator EndScreenFade()
    {
        endScreenViewed = false;
        menuAnimator.Play("EndScreenFade");
        yield return new WaitForSeconds(2f);
        endScreenClass.SetActive(false);
        menuClass.SetActive(true);
        StartCoroutine(EnableMenu(false));
    }

    /// <summary> Fade from black Overlay. </summary>
    IEnumerator OverlayWait()
    {
        yield return new WaitForSeconds(2);
        Destroy(overlay);
        overlayFinished = true;
    }

    /// <summary> Enables the Pause Menu when called. </summary>
    /// <param name="Paused"> True if the game is unpaused. </param>
    void PauseMenu(bool Paused)
    {
        if(!Paused && GameStarted)
        {
            GamePaused = true;
            GameStarted = false;
            pauseScreenClass.SetActive(true);
            waveManager.waveText.gameObject.SetActive(false);
            Time.timeScale = 0;
            pauseJingle.Play();
        }
        else
        {
            GamePaused = false;
            GameStarted = true;
            pauseScreenClass.SetActive(false);
            waveManager.waveText.gameObject.SetActive(true);
            Time.timeScale = 1;
        }
    }

    /// <summary> Dampens sound effects during the duration of an audio file. </summary>
    IEnumerator MusicDampen(AudioSource audio)
    {
        jinglePlaying = true;
        yield return new WaitForSeconds(audio.clip.length);
        jinglePlaying = false;
    }
}