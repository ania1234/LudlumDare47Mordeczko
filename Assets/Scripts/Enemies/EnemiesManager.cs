using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    public List<GameObject> enemyWaves;

    private IEnumerator Start()
    {
        while (GameManager.instance == null)
        {
            yield return null;
        }
        GameManager.instance.onPassChanged += Instance_onPassChanged;

        enemyWaves[0].SetActive(true);
    }

    private void Instance_onPassChanged(int obj)
    {
        var currentWave = obj % enemyWaves.Count;

        for (int i = 0; i < enemyWaves.Count; i++)
        {
            enemyWaves[i].SetActive(i==currentWave);
        }
    }
}
