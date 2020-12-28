using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool isDormant;
    public string itemRequiredToKill;
    public GameObject bubble;
    public SpriteRenderer itemSprite;
    private IEnumerator Start()
    {
        while (GameManager_old.instance == null)
        {
            yield return null;
        }

        itemSprite.sprite = Shop.instance.items.Find(o => o.items[0].name == itemRequiredToKill).items[0].icon;
        GameManager_old.instance.onDayTimeChanged += Instance_onDayTimeChanged;
    }

    //Tutaj budzi sie przeciwnik
    private void Instance_onDayTimeChanged(DayTimeEnum obj)
    {
        isDormant = obj == DayTimeEnum.day;
        if(obj == DayTimeEnum.night)
        {
            bubble.gameObject.SetActive(false);
        
       }
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
