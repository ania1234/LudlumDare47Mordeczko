using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Shop : MonoBehaviour
{
    public bool randomItems = true;
    public List<ItemInfo> items;
    public List<ShopSlot> slots;
    public GameObject slotPlace;

    private IEnumerator Start()
    {
        while (GameManager.instance == null)
        {
            yield return null;
        }

        GameManager.instance.onDayTimeChanged += OnDayTimeChanged;
        OnDayTimeChanged(GameManager.instance.DayTime);
    }

    private void OnDayTimeChanged(DayTimeEnum value)
    {
        if (value == DayTimeEnum.day)
        {
            SetShop(); 
        }
        else
        {
            for (int slot = 0; slot < slots.Count; slot++)
            {
                slots[slot].ReturnToSlot();
            }
        }
        slotPlace.SetActive(value == DayTimeEnum.day);
    }

    void SetShop()
    {
        if (randomItems)
        {
            System.Random rnd = new System.Random();
            var randomizedList = from item in items
                                 orderby rnd.Next()
                                 select item;
            items = randomizedList.ToList();
        }
        for (int slot = 0; slot < slots.Count; slot++)
        {
            slots[slot].Init(items[slot]);
        }
    }
}
