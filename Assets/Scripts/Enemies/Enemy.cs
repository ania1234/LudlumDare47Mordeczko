using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool isDormant;
    public string itemRequiredToKill;

    private IEnumerator Start()
    {
        while (GameManager.instance == null)
        {
            yield return null;
        }

        GameManager.instance.onDayTimeChanged += Instance_onDayTimeChanged;
    }

    //Tutaj budzi sie przeciwnik
    private void Instance_onDayTimeChanged(DayTimeEnum obj)
    {
        isDormant = obj == DayTimeEnum.day;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDormant)
        {
            return;
        }

        else if(collision.gameObject == Player.instance.gameObject)
        {
            if (Player.instance.HasItemEquipped(itemRequiredToKill))
            {
                //StartCoroutine(GoingToDormantFireworks);
                isDormant = true;
            }
            else
            {
                //StartCoroutine(BeatingFireworks);
                Player.instance.TakeBeatingFrom(this);
            }
        }
    }
}
