using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform spawnOrigin;

    public bool finished = true;

    public WaveManager waveManager;

    public float delay;

    int spawnNum;

    [System.Serializable]
    public class wave
    {
        [System.Serializable]
        public struct enemySpawn
        {
            public float timer;

            public GameObject enemy;

            public Vector2 spawnOffset;
        }

        public enemySpawn[] enemySpawns;
    }

    public wave[] waves;
    public void StartWave()
    {
        spawnNum = 0;
        delay = 0;
        finished = false;
    }

    public void RestartWave()
    {
        spawnNum = 0;
        delay = 0;
        finished = true;
    }

    private void Update()
    {
        if (waveManager.playing && delay > 0 && finished == false)
        {
            delay -= Time.deltaTime;
        }

        if(waveManager.playing)
        {
            if (delay <= 0 && spawnNum < waves[waveManager.curWave - 1].enemySpawns.Length && finished == false)
            {
                //Debug.Log("spawned Enemy!");
                GameObject e = Instantiate(waves[waveManager.curWave - 1].enemySpawns[spawnNum].enemy, spawnOrigin.position + new Vector3(waves[waveManager.curWave - 1].enemySpawns[spawnNum].spawnOffset.x, waves[waveManager.curWave - 1].enemySpawns[spawnNum].spawnOffset.y, 0), Quaternion.identity);
                delay = waves[waveManager.curWave - 1].enemySpawns[spawnNum].timer;
                waveManager.liveEnemies.Add(e);

                spawnNum += 1;
            }
            else if (spawnNum >= waves[waveManager.curWave - 1].enemySpawns.Length)
            {
                finished = true;
                //Debug.Log(this.name + " Finished Spawning");
            }
        }

        if (waveManager.playing && spawnNum == waves[waveManager.curWave - 1].enemySpawns.Length)
        {
            finished = true;
        }
    }
}
