using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthNumberTracker : MonoBehaviour
{
    public List<GameObject> hearts;
    public AudioSource ugh;

    private IEnumerator Start()
    {
        while (Player.instance == null)
        {
            yield return null;
        }
        Player.instance.onHealthChanged += Instance_onHealthChanged;
    }

    private void Instance_onHealthChanged(int health)
    {
        for(int i =0; i<hearts.Count; i++)
        {
            if(hearts[i].activeSelf && i >= health)
            {
                ugh.Play();
            }

            hearts[i].SetActive(i < health);
        }
    }
}
