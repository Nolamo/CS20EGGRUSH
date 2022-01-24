using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpRandomizer : MonoBehaviour
{
    public List<GameObject> items = new List<GameObject>();

    public GameObject[] slots;

    public List<GameObject> spawnedItems = new List<GameObject>();

    public WaveManager mngr;

    bool itemsSpawned;

    private void Update()
    {
        if (itemsSpawned)
        {
            foreach(GameObject b in spawnedItems)
            {
                if (b == null)
                {
                    ClearItems();

                    for (int i = 0; i < items.ToArray().Length; i++)
                    {
                        if(items[i].name == b.name)
                        {
                            items.RemoveAt(i);
                        }
                    }

                    itemsSpawned = false;
                }
            }
        }
    }

    public void ClearItems()
    {
        mngr.playAutomatically = true;

        foreach (GameObject i in spawnedItems)
        {
            if(i!= null)
            {
                Destroy(i);
            }
        }

        spawnedItems.Clear();
    }

    public void Randomize()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            spawnedItems.Add(Instantiate(items[Random.Range(0, items.ToArray().Length)], slots[i].transform.position, Quaternion.identity));
        }
        itemsSpawned = true;
    }
}
