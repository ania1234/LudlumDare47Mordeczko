using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWave : MonoBehaviour
{
    public List<int> enemiesInThisWaveIndicators1;
    public List<int> enemiesInThisWaveIndicators2;
    public List<int> enemiesInThisWaveIndicators3;
    System.Random random;
    private List<int> choosenWave;
    public List<Vector3> spawnedenemyPlaces;
    public EnemiesManager manager;
    public int timeForThisWave;
    public void StartWave()
    {
        random = new System.Random();
        ChooseWave();
        ShuffleWave();
        SpawnEnemies();
        GameManager.instance.dayDuration = timeForThisWave;
    }

    private void SpawnEnemies()
    {
        int i = 0;
        foreach (var e in choosenWave)
        {

            var enemy = Instantiate(manager.enemyPrefabs[e]);
            enemy.transform.parent = this.transform;
            enemy.transform.localScale = new Vector3(0.5f, 0.5f, 1);
            enemy.transform.localPosition = spawnedenemyPlaces[i];
            i++;

        }
    }
    private void OnDrawGizmosSelected()
    {
        for(int i = 0; i < spawnedenemyPlaces.Count; i++)
        {
            Gizmos.DrawCube(spawnedenemyPlaces[i] + this.transform.position, new Vector3(0.1f, 0.1f, 0.1f));

        }
    }

    private void ChooseWave()
    {
      var i = random.Next(0, 3);
        if (i == 0)
        {
            choosenWave = enemiesInThisWaveIndicators1;
        }
        else if (i==1)
        {
            choosenWave = enemiesInThisWaveIndicators2;

        }
        else
        {
            choosenWave = enemiesInThisWaveIndicators3;

        }
        Debug.Log("random enemylist:" + i);
    }

    private void ShuffleWave()
    {
       choosenWave=(List<int>) Shuffle(choosenWave);
    }

    public IList<T> Shuffle<T>(IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
        return list;
    }
}
