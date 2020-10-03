using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool isDormant;
    public string itemRequiredToKill;

    public void OnCollisionEnter2D(Collision2D collision)
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
