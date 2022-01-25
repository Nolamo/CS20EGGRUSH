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
        // check if there are changes in the list, if so, clear list.
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
                            items.RemoveAt(i);
                    }

                    itemsSpawned = false;
                }
            }
        }
    }

    /// <summary> When called, clear items that remain. Primarily called when the player picks a PowerUp to clear out other choices. </summary>
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

    /// <summary> Spawns a random item from the item pool. </summary>
    public void Randomize()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            spawnedItems.Add(Instantiate(items[Random.Range(0, items.ToArray().Length)], slots[i].transform.position, Quaternion.identity));
        }
        itemsSpawned = true;
    }
}
