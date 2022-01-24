using System.Collections;
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

    public void OnSubmit()
    {
        Debug.Log("OnSubmit");
        if(!GameStarted)
        {
            if (overlayFinished && !CoroutineStarted)
            {
                DoOnce();
                StartCoroutine(DisableMenu());
            }
            if (endScreenViewed)
            {
                StartCoroutine(EndScreenFade());
            }
        }
    }

    void Update()
    {
        // Timer Stuff
        {
            seconds = (int)GameTime % 60;
            minutes = (int)GameTime / 60;
            hours = (int)GameTime / 3600;

            if (GameStarted)
            {
                GameTime += Time.unscaledDeltaTime;
            }
            if (minutes == 0)
            {
                minutesString = "00";
            }
            else if (minutes < 10)
            {
                minutesString = "0" + minutes.ToString();
            }
            else
            {
                minutesString = minutes.ToString();
            }
            if (minutes == 60)
            {
                minutesString = "00";
            }
            if (hours == 0)
            {
                hoursString = "00";
            }
            if (hours < 10)
            {
                hoursString = "0" + hours.ToString();
            }
            else
            {
                hoursString = hours.ToString();
            }
            if (seconds == 0)
            {
                secondsString = "00";
            }
            else if (seconds < 10)
            {
                secondsString = "0" + seconds.ToString();
            }
            else
            {
                secondsString = seconds.ToString();
            }
        }
    }

    void DoOnce()
    {
        if (!CoroutineStarted)
        {
            playerMovement.input.Gameplay.Submit.Disable();
            CoroutineStarted = true;
            menuAnimator.Play("MainMenuTrigger");
        }
    }

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
            {
                Destroy(i);
            }
        }
        waveManager.curWave = 1;
        waveManager.firstWave = true;
        menuAnimator.Play("MainMenuEnable");
        playerMovement.ResetPlayer(true);
        playerMovement.input.Gameplay.Submit.Enable();
        yield return new WaitForSeconds(2);
        CoroutineStarted = false;
    }

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
        playerMovement.input.Gameplay.Submit.Enable();
        yield return new WaitForSeconds(3);
        endScreenViewed = true;
        hours = 0;
        minutes = 0;
        seconds = 0;
    }

    IEnumerator EndScreenFade()
    {
        endScreenViewed = false;
        menuAnimator.Play("EndScreenFade");
        yield return new WaitForSeconds(2f);
        endScreenClass.SetActive(false);
        menuClass.SetActive(true);
        StartCoroutine(EnableMenu(false));
    }

    IEnumerator OverlayWait()
    {
        yield return new WaitForSeconds(2);
        Destroy(overlay);
        overlayFinished = true;
    }

    public void PauseMenu(bool Paused)
    {
        Debug.Log("OnPause");
        if (!Paused && GameStarted)
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

    IEnumerator MusicDampen(AudioSource audio)
    {
        jinglePlaying = true;
        yield return new WaitForSeconds(audio.clip.length);
        jinglePlaying = false;
    }
}