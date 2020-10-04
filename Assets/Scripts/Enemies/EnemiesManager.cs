using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    public List<EnemyWave> enemyWaves;
    public List<Enemy> enemyPrefabs;
    private IEnumerator Start()
    {
        while (GameManager.instance == null)
        {
            yield return null;
        }
        GameManager.instance.onPassChanged += Instance_onPassChanged;

    }

    private void Instance_onPassChanged(int obj)
    {
        var currentWave = obj % enemyWaves.Count;
        if(obj == 2)
        {
            Shop.instance.items.Add(Shop.instance.item1ToUnlock);
            Shop.instance.slotPlace.gameObject.SetActive(true);
            Shop.instance.slots[3].gameObject.SetActive(true);
        }
        if(obj == 4)
        {

            Shop.instance.items.Add(Shop.instance.item2ToUnlock);
            Shop.instance.slotPlace.gameObject.SetActive(true);
            Shop.instance.slots[4].gameObject.SetActive(true);
        }
        for (int i = 0; i < enemyWaves.Count; i++)
        {
            if (i == currentWave)
            {
                enemyWaves[i].gameObject.SetActive(true);
                enemyWaves[i].StartWave();

            }
            else
            {

                enemyWaves[i].gameObject.SetActive(false);
            }
        }
    }
}
