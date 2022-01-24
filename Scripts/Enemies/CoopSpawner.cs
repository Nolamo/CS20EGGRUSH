using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoopSpawner : MonoBehaviour
{

    public Transform spawnpoint;
    //public float time = 2;

    public GameObject[] enemies;

    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(SpawnEnemy());

    }

    IEnumerator SpawnEnemy()
    {

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
