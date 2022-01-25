using System.Collections;
using UnityEngine;

/// <summary> Generic enemy spawner. </summary>
public class CoopSpawner : MonoBehaviour
{

    public Transform spawnpoint;
    public GameObject[] enemies;

    void Start()
    {

        StartCoroutine(SpawnEnemy());

    }

    // Spawns enemies in cue every 2-4 seconds.
    IEnumerator SpawnEnemy()
    {
        // if there are more than 10 enemies on screen, stop spawning until there's less than 10.
        GameObject[] count = GameObject.FindGameObjectsWithTag("Enemy");
        if (count.Length >= 10)
        {
            StartCoroutine(CountEnemies());
            yield break;
        }
        else if (count.Length <= 10)
        {
            Vector3 spawn = spawnpoint.transform.position;
            Instantiate(enemies[Random.Range(0, enemies.Length)], spawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(2f, 4f));
            StartCoroutine(SpawnEnemy());
        }


    }

    // what to do while waiting to spawn enemies.
    IEnumerator CountEnemies()
    {
        GameObject[] count = GameObject.FindGameObjectsWithTag("Enemy");

        if (count.Length <= 8)
        {
            StartCoroutine(SpawnEnemy());
            yield break;
        }
        else
        {
            yield return new WaitForSeconds(1f);
            StartCoroutine(CountEnemies());
        }

    }

}
