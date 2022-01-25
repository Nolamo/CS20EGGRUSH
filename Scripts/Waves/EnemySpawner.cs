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
        // contains info on the amount of time to wait before spawning the enemy, and the enemy to spawn. Spawn position offset also included.
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
        // count down until spawning more enemies.
        if (waveManager.playing && delay > 0 && finished == false)
            delay -= Time.deltaTime;

        if(waveManager.playing)
        {
            if (delay <= 0 && spawnNum < waves[waveManager.curWave - 1].enemySpawns.Length && finished == false)
            {
                //Debug.Log("spawned Enemy!");
                GameObject e = Instantiate(waves[waveManager.curWave - 1].enemySpawns[spawnNum].enemy, spawnOrigin.position + new Vector3(waves[waveManager.curWave - 1].enemySpawns[spawnNum].spawnOffset.x, waves[waveManager.curWave - 1].enemySpawns[spawnNum].spawnOffset.y, 0), Quaternion.identity);
                delay = waves[waveManager.curWave - 1].enemySpawns[spawnNum].timer;
                waveManager.liveEnemies.Add(e); // add newly spawned enemy to live enemy count.

                spawnNum += 1;
            }
            // if there's no more enemies left to spwan, allow wave to end when no enemies are left.
            else if (spawnNum >= waves[waveManager.curWave - 1].enemySpawns.Length)
                finished = true;
        }
        // ""
        if (waveManager.playing && spawnNum == waves[waveManager.curWave - 1].enemySpawns.Length)
            finished = true;
    }
}
