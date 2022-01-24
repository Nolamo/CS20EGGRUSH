using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    [System.Serializable]
    public class wave
    {
        public float waveTimer;
        public bool waveReward;
    }

    public MenuManager menuManager;

    public wave[] waves;

    public int curWave;

    public int finalWave;

    public bool playing;

    public bool waveStarted = false;

    public float delay;

    public EnemySpawner[] spawners;

    public List<GameObject> liveEnemies = new List<GameObject>();

    public PowerUpRandomizer randomizer;

    public Text waveText;

    public bool playAutomatically;

    [HideInInspector]
    public bool firstWave = true; // I don't know how else to fix a bug related to this without spending more time then I have rn

    private void Start()
    {
        firstWave = true;
    }

    public bool finishedSpawning(EnemySpawner[] spawns)
    {
        foreach (EnemySpawner s in spawns) if (!s.finished) return false;
        return true;
    }

    bool gameFinished;

    private void Update()
    {

        playing = delay <= 0;

        if (playing && !waveStarted && finishedSpawning(spawners) && playAutomatically)
        {
            StartWave();
        }


        if (delay > 0 && !gameFinished)
        {
            delay -= Time.deltaTime;
        }

        if (liveEnemies.ToArray().Length == 0 && finishedSpawning(spawners) && waveStarted)
        {
            FinishWave();
        }

        for (int i = 0; i < liveEnemies.ToArray().Length; i++)
        {
            if (!liveEnemies[i])
            {
                liveEnemies.RemoveAt(i);
                break;
            }
        }

    }

    private void OnValidate()
    {
        curWave = Mathf.Clamp(curWave, 1, finalWave);
    }

    void StartWave()
    {
        Debug.LogWarning("Wave " + curWave + " Started!");

        gameFinished = curWave == finalWave && finishedSpawning(spawners);

        waveStarted = true;

        

        if (gameFinished)
        {
            FinishGame();
        }
        else
        {
            if(firstWave == true)
            {
                curWave = 1;
                waveText.text = "- Wave " + (curWave) + " -";
                firstWave = false;
            }
            else
            {
                curWave += 1;
                waveText.text = "- Wave " + (curWave) + " -";
            }

            for (int i = 0; i < spawners.Length; i++)
            {
                spawners[i].StartWave();
            }
        }
        
    }

    void FinishWave()
    {
        if (waves[curWave - 1].waveReward)
        {
            randomizer.Randomize();
            playAutomatically = false;
        }

        delay = waves[curWave - 1].waveTimer;
        waveStarted = false;
    }

    public void KillAllEntities()
    {
        for (int i = 0; i < liveEnemies.ToArray().Length; i++)
        {
            Destroy(liveEnemies[i].gameObject);
        }
    }

    void FinishGame()
    {
        waveText.text = "";
        menuManager.menuClass.SetActive(true);
        StartCoroutine(menuManager.EndScreen());
        gameFinished = false;
    }
}
